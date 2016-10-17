using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Download;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Newtonsoft.Json;
using NotedUI.Models;
using NotedUI.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NotedUI.DataStorage
{
    public class GoogleDriveStorage : ICloudStorage
    {
        private const string GROUP_DIRECTORY = "Groups.txt";

        /// <summary>
        /// This permission only allows viewing of files I have created or opened with this app
        /// </summary>
        private string[] _scopes = { DriveService.Scope.DriveFile };
        private string _appName = "Noted";
        private UserCredential _credentials;
        private DriveService _service;
        private File _directory;
        private bool _isConnected;
        private string _groupFileID = null;

        public GoogleDriveStorage()
        { }

        public async Task Connect()
        {
            _credentials = await GetCredentials();

            _service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = _credentials,
                ApplicationName = _appName,
            });

            _isConnected = true;

            _directory = InitializeAndGetDirectory();
        }

        public bool IsConnected()
        {
            return _isConnected;
        }

        public async Task<Dictionary<string, Note>> GetAllNotes()
        {
            var notes = new Dictionary<string, Note>();
            var files = await GetFiles(null, GROUP_DIRECTORY);

            foreach (var file in files)
                notes.Add(file.Id, new Note(file.Id, file.ModifiedTime));

            return notes;
        }

        public async Task GetNoteWithContent(Note note)
        {
            var file = GetFile(note.CloudKey);

            var latestNote = Note.FromJSON(await GetFileContent(note.CloudKey));

            // Grab the latest content from the cloud note
            note.Content = latestNote.Content;
            note.ID = latestNote.ID;
            note.Group = latestNote.Group;
        }

        public async Task AddNote(Note note)
        {
            // Create new file first with default name then rename it to the ID.txt after
            var file = CreateNewFile("temp.txt", note.ToJSON(), _directory.Id);
            var patchedFile = await PatchFile($"{ file.Id }.txt", file.Id);

            note.CloudKey = file.Id;
            note.LastModified = file.ModifiedTime;
        }

        public async Task UpdateNote(Note note)
        {
            // Update note and get the last modified date from the return data
            var file = await UpdateFile($"{ note.CloudKey }.txt", note.CloudKey, note.ToJSON());

            note.LastModified = file.ModifiedTime;
        }

        public async Task DeleteNote(Note note)
        {
            var request = _service.Files.Delete(note.CloudKey);
            await request.ExecuteAsync();
        }


        public async Task<bool> DoGroupsNeedToBeUpdated(GroupCollection groups)
        {
            var files = await GetFiles(GROUP_DIRECTORY);

            if (files.Count == 0)
                return false;

            _groupFileID = files[0].Id;
            return groups.LastModified != files[0].ModifiedTime;
        }

        public async Task<GroupCollection> GetAllGroups()
        {
            var groupFile = GetFile(_groupFileID);

            return GroupCollection.FromJSON(await GetFileContent(_groupFileID));
        }

        public async Task UpdateAllGroups(GroupCollection allGroups)
        {
            // Perhaps revisit this section, might want to do something cleverer
            if (_groupFileID == null)
                return;

            await UpdateFile(GROUP_DIRECTORY, _groupFileID, allGroups.ToJSON());
        }


        private async Task<UserCredential> GetCredentials()
        {
            string credPath = AppDomain.CurrentDomain.BaseDirectory + _appName;

            try
            {
                UserCredential userCredential;
                using (var stream = new System.IO.MemoryStream(Encoding.UTF8.GetBytes(Properties.Resources.ClientID)))
                {
                    userCredential = GoogleWebAuthorizationBroker.AuthorizeAsync(stream,
                                                                                 _scopes,
                                                                                 "user",
                                                                                 CancellationToken.None,
                                                                                 new FileDataStore(credPath, true)).Result;
                }

                if (IsCredentialExpired(userCredential))
                    await userCredential.RefreshTokenAsync(CancellationToken.None);

                return userCredential;
            }
            catch (AggregateException ae)
            {
                var googleEx = ae.InnerException as TokenResponseException;
                Debug.WriteLine("Message: " + googleEx.Message);
            }

            return null;
        }

        private File InitializeAndGetDirectory()
        {
            var dir = GetRootDirectory();

            if (dir != null)
                return dir;
            else
                return CreateDirectoryAndInitializeData();
        }

        private File GetRootDirectory()
        {
            var request = _service.Files.List();
            request.Q = "mimeType='application/vnd.google-apps.folder' and trashed = false";
            FileList folders = request.Execute();

            foreach (var folder in folders.Files)
            {
                if (folder.Name.Contains(_appName))
                    return folder;
            }

            return null;
        }

        private File CreateDirectoryAndInitializeData()
        {
            var noteDirectory = CreateNoteDirectory();

            // Create groups file - make it empty as default
            _groupFileID = CreateNewFile(GROUP_DIRECTORY, new GroupCollection().ToJSON(), noteDirectory.Id).Id;

            return noteDirectory;
        }

        private File CreateNoteDirectory()
        {
            // Create metaData for a new Directory
            File body = new File();
            body.Name = _appName;
            body.MimeType = "application/vnd.google-apps.folder";
            body.Parents = new List<string>() { "root" };

            try
            {
                FilesResource.CreateRequest request = _service.Files.Create(body);
                return request.Execute();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private bool IsCredentialExpired(UserCredential credential)
        {
            return DateTime.Now >= credential.Token.Issued.AddSeconds((double)credential.Token.ExpiresInSeconds);
        }

        private File GetFile(string fileID)
        {
            var request = _service.Files.Get(fileID);
            request.Fields = "id,modifiedTime,name,version";
            File file = request.Execute();

            return file;
        }

        private async Task<IList<File>> GetFiles(string nameEqual = null, string nameNotEqual = null)
        {
            FilesResource.ListRequest request = _service.Files.List();
            request.Q = "mimeType != 'application/vnd.google-apps.folder' and trashed = false";
            if (nameEqual != null)
                request.Q += $" and name = '{nameEqual}'";
            if (nameNotEqual != null)
                request.Q += $" and name != '{nameNotEqual}'";
            request.Fields = "files(id,modifiedTime,name,version)";
            FileList files = await request.ExecuteAsync();

            return files.Files;
        }

        private async Task<string> GetFileContent(string fileID)
        {
            int numRetries = 0;

            while (true)
            {
                try
                {
                    return await GetNoteContentTask(fileID);
                }
                catch (Exception ex)
                {
                    if (numRetries >= 5)
                        throw ex;

                    numRetries++;
                }
            }
        }

        private async Task<string> GetNoteContentTask(string fileID)
        {
            TaskCompletionSource<string> tcs = new TaskCompletionSource<string>();

            var fileId = fileID;
            var request = _service.Files.Get(fileId);
            var stream = new System.IO.MemoryStream();

            request.MediaDownloader.ProgressChanged += progress =>
            {
                if (progress.Status == DownloadStatus.Completed)
                {
                    stream.Position = 0;
                    tcs.SetResult(new System.IO.StreamReader(stream).ReadToEnd());
                }
                else if (progress.Status == DownloadStatus.Failed)
                {
                    tcs.SetException(new Exception("File download failed"));
                }
            };

            request.Download(stream);

            return await tcs.Task;
        }

        private File CreateNewFile(string filename, string content, string directoryId)
        {
            File body = new File();
            body.Name = filename;
            body.Parents = new List<string>() { directoryId };

            // File's content.
            var stream = new System.IO.MemoryStream(Encoding.UTF8.GetBytes(content));
            try
            {
                FilesResource.CreateMediaUpload request = _service.Files.Create(body, stream, String.Empty);
                request.Fields = "id,modifiedTime,name,version";
                request.Upload();
                return request.ResponseBody;
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: " + e.Message);
                return null;
            }
        }

        private async Task<File> UpdateFile(string filename, string fileID, string content)
        {
            File body = new File();
            body.Name = filename;

            // File's content.
            var stream = new System.IO.MemoryStream(Encoding.UTF8.GetBytes(content));
            try
            {
                var request = _service.Files.Update(body, fileID, stream, String.Empty);
                request.Fields = "id,modifiedTime,name,version";
                await request.UploadAsync();
                return request.ResponseBody;
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: " + e.Message);
                return null;
            }
        }

        private async Task<File> PatchFile(string filename, string fileID)
        {
            File body = new File();
            body.Name = filename;

            try
            {
                var request = _service.Files.Update(body, fileID);
                request.Fields = "id,modifiedTime,name,version";
                return await request.ExecuteAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: " + e.Message);
                return null;
            }
        }
    }
}
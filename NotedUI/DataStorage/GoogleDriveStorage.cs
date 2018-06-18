using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
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
using System.Linq;
using System.Net;
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
        private string[] _scopes = 
        {
            DriveService.Scope.DriveFile
        };
        private string _appName = "Noted_ChrisBJohnsonDev";
        private UserCredential _credentials;
        private DriveService _service;
        private File _directory;
        private bool _isConnected;
        private string _groupFileID = null;
        private string _username;

        public event Action InternetConnected;

        public GoogleDriveStorage(string username)
        {
            _username = username;
        }

        public async Task<bool> Connect()
        {
            var task = GetCredentials(_username);

            if (await Task.WhenAny(task, Task.Delay(60000)) == task)
                _credentials = task.Result;
            else
                return false;

            _service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = _credentials,
                ApplicationName = _appName,
            });

            _isConnected = true;
            _directory = InitializeAndGetDirectory();

            return true;
        }

        public bool IsConnected()
        {
            // Attempt to reconnect regularly
            if (!_isConnected && IsInternetConnected())
                Task.Run(async () =>
                {
                    if (await Connect())
                        InternetConnected?.Invoke();
                });

            return _isConnected;
        }

        public async Task<Dictionary<string, Note>> GetAllNotes()
        {
            if (!_isConnected)
                return new Dictionary<string, Note>();

            var notes = new Dictionary<string, Note>();
            var files = await GetFiles(_directory.Id, null, GROUP_DIRECTORY);

            if (files == null)
            {
                _isConnected = false;
                return null;
            }

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
            if (!_isConnected)
                return;

            // Update note and get the last modified date from the return data
            var file = await UpdateFile($"{ note.CloudKey }.txt", note.CloudKey, note.ToJSON());

            if (file == null)
            {
                _isConnected = false;
                return;
            }

            note.LastModified = file.ModifiedTime;
        }

        public async Task DeleteNote(Note note)
        {
            var request = _service.Files.Delete(note.CloudKey);
            await request.ExecuteAsync();
        }


        public async Task<bool> DoGroupsNeedToBeUpdated(GroupCollection groups)
        {
            if (!_isConnected)
                return false;

            var files = await GetFiles(_directory.Id, GROUP_DIRECTORY);

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

        public bool IsInternetConnected()
        {
            try
            {
                using (var client = new WebClient())
                    using (var stream = client.OpenRead("http://drive.google.com"))
                        return true;
            }
            catch
            {
                return false;
            }
        }

        private async Task<UserCredential> GetCredentials(string username)
        {
            try
            {
                UserCredential userCredential;
                using (var stream = new System.IO.MemoryStream(Encoding.UTF8.GetBytes(Properties.Resources.ClientID)))
                {
                    userCredential = await GoogleWebAuthorizationBroker.AuthorizeAsync(stream,
                                                                                       _scopes,
                                                                                       username,
                                                                                       CancellationToken.None,
                                                                                       new FileDataStore("chrisbjohnsondev.noted"));
                }

                if (IsCredentialExpired(userCredential))
                    await userCredential.RefreshTokenAsync(CancellationToken.None);

                return userCredential;
            }
            catch (AggregateException ae)
            {
                if (ae.InnerException.GetType() == typeof(TokenResponseException))
                {
                    Debug.WriteLine("Message: " + ae.InnerException.Message);
                }
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
            request.Q = "mimeType='application/vnd.google-apps.folder' and trashed = false and 'root' in parents";
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
            return DateTime.Now >= credential.Token.IssuedUtc.AddSeconds((double)credential.Token.ExpiresInSeconds);
        }

        public File GetFile(string fileID)
        {
            var request = _service.Files.Get(fileID);
            request.Fields = "id,modifiedTime,name,version";

            return request.Execute();
        }

        private async Task<IList<File>> GetFiles(string folderId, string nameEqual = null, string nameNotEqual = null)
        {
            FilesResource.ListRequest request = _service.Files.List();
            request.Q = $"mimeType != 'application/vnd.google-apps.folder' and trashed = false and '{folderId}' in parents";
            if (nameEqual != null)
                request.Q += $" and name = '{nameEqual}'";
            if (nameNotEqual != null)
                request.Q += $" and name != '{nameNotEqual}'";
            request.Fields = "files(id,modifiedTime,name,version)";

            try
            {
                FileList files = await request.ExecuteAsync();

                return files.Files;
            }
            catch
            {
                return null;
            }
        }

        public async Task<string> GetFileContent(string fileID)
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

                Task task = request.UploadAsync();
                if (await Task.WhenAny(task, Task.Delay(5000)) == task)
                    return request.ResponseBody;
                else
                    return null;
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: " + e.Message);
                return null;
            }
            finally
            {
                stream.Dispose();
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

        public async Task<Dictionary<string, InstallFile>> GetFilesForLatestVersion()
        {
            var files = await GetFilesRecursively(_service, "0B6J4GdPPuCf4NTNUSlI5RkQxY2s", "", new List<InstallFile>());

            return files.ToDictionary(f => f.Filename, f => f);
        }

        private async Task<List<InstallFile>> GetFilesRecursively(DriveService service, string folderID, string path, List<InstallFile> currentFiles)
        {
            var newFiles = await GetFiles(folderID);

            foreach (var item in newFiles)
            {
                if (item.MimeType == "application/vnd.google-apps.folder")
                {
                    await GetFilesRecursively(service, item.Id, System.IO.Path.Combine(path, item.Name), currentFiles);
                }
                else
                {
                    currentFiles.Add(new InstallFile()
                    {
                        UpdateType = eUpdateType.None,
                        Filename = System.IO.Path.Combine(path, item.Name),
                        LastModified = (DateTime)item.ModifiedTime,
                        CloudID = item.Id
                    });
                }
            }

            return currentFiles;
        }

        public async Task<bool> DownloadFile(string cloudID, string filename)
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

            var request = _service.Files.Get(cloudID);
            var stream = new System.IO.MemoryStream();

            request.MediaDownloader.ProgressChanged += progress =>
            {
                if (progress.Status == DownloadStatus.Completed)
                {
                    using (var file = new System.IO.FileStream(filename, System.IO.FileMode.Create, System.IO.FileAccess.Write))
                        stream.WriteTo(file);

                    tcs.SetResult(true);
                }
                else if (progress.Status == DownloadStatus.Failed)
                {
                    tcs.SetException(new Exception("File download failed"));
                }
            };

            request.Download(stream);

            return await tcs.Task;
        }
    }
}
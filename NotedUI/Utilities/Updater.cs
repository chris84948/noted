using Google.Apis.Drive.v3;
using Google.Apis.Services;
using NotedUI.DataStorage;
using NotedUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NotedUI.Utilities
{
    class Updater
    {
        public static void CheckForUpdates(ICloudStorage cloudStorage)
        {
            Task.Run(async () =>
            {
                //string saltKey = "0B6J4GdPPuCf4Vnc2MUw3d2dqM2s";
                //var salt = await cloudStorage.GetFileContent(saltKey);

                //var fileChanges = await GetFileChanges(cloudStorage);

                //foreach (var file in fileChanges.Where(f => f.UpdateType == eUpdateType.Add || f.UpdateType == eUpdateType.Update))
                //    await cloudStorage.DownloadFile(file.CloudID, System.IO.Path.Combine("Update", file.Filename));
            });
        }

        public async static Task<List<InstallFile>> GetFileChanges(ICloudStorage cloudStorage)
        {
            List<InstallFile> localFiles = GetCurrentInstallFiles(Assembly.GetEntryAssembly().Location);
            Dictionary<string, InstallFile> remoteFiles = await cloudStorage.GetFilesForLatestVersion();

            foreach (InstallFile localFile in localFiles)
            {
                if (remoteFiles.ContainsKey(localFile.Filename))
                {
                    var remoteFile = remoteFiles[localFile.Filename];

                    // File as changed, update it
                    if (!remoteFile.LastModified.DateMatches(localFile.LastModified) && remoteFile.LastModified > localFile.LastModified)
                    {
                        localFile.CloudID = remoteFile.CloudID;
                        localFile.UpdateType = eUpdateType.Update;
                    }

                    remoteFiles.Remove(remoteFile.Filename);
                }
                else
                {
                    // File doesn't exist in cloud, delete it
                    localFile.UpdateType = eUpdateType.Delete;
                }
            }

            // File doesn't exist locally, add it
            foreach (InstallFile remoteFile in remoteFiles.Values)
            {
                remoteFile.UpdateType = eUpdateType.Add;
                localFiles.Add(remoteFile);
            }

            return localFiles.Where(f => f.UpdateType != eUpdateType.None).ToList();
        }

        private static List<InstallFile> GetCurrentInstallFiles(string directory)
        {
            var files = new List<InstallFile>();

            foreach (System.IO.FileInfo file in new System.IO.DirectoryInfo(directory).EnumerateFiles("*", System.IO.SearchOption.AllDirectories))
            {
                files.Add(new InstallFile()
                {
                    UpdateType = eUpdateType.None,
                    Filename = file.FullName.Replace(directory + "\\", ""),
                    LastModified = file.LastWriteTime
                });
            }

            return files;
        }
    }
}
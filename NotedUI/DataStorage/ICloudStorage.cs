using NotedUI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NotedUI.DataStorage
{
    public interface ICloudStorage
    {
        Task Connect();
        bool IsConnected();

        Task<Dictionary<string, Note>> GetAllNotes();
        Task GetNoteWithContent(Note note);
        Task AddNote(Note note);
        Task UpdateNote(Note note);
        Task DeleteNote(Note note);

        Task<bool> DoGroupsNeedToBeUpdated(GroupCollection groups);
        Task<GroupCollection> GetAllGroups();
        Task UpdateAllGroups(GroupCollection groups);

        Task<Dictionary<string, InstallFile>> GetFilesForLatestVersion();
        Task<bool> DownloadFile(string cloudID, string filename);
    }
}

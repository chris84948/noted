using NotedUI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NotedUI.DataStorage
{
    interface ILocalStorage
    {
        Task Initialize();

        Task<List<Note>> GetAllNotes();

        Task<long> AddNote(string folderName);
        Task UpdateNote(Note note);
        Task DeleteNote(Note note);

        Task<Folder> GetFolder(string folderName);
        Task<List<Folder>> GetAllFolders();
        Task<long> AddFolder(string folderName);
        Task UpdateFolder(string folderName);
    }
}

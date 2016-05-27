using NotedUI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NotedUI.DataStorage
{
    interface ILocalStorage
    {
        Task Initialize();

        Task<List<Note>> GetAllNotes();
        Task<long> AddNote(string groupName);
        Task<long> AddNote(Note note);
        Task UpdateNote(Note note);
        Task DeleteNote(Note note);

        Task<Group> GetGroup(string groupName);
        Task<List<Group>> GetAllGroups();
        Task<long> AddGroup(string groupName);
        Task UpdateGroup(string oldGroupName, string newGroupName);
        Task DeleteGroup(string groupName);
    }
}

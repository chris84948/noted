using NotedUI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NotedUI.DataStorage
{
    interface ICloudStorage
    {
        Task Connect();
        bool IsConnected();

        Dictionary<string, Note> GetAllNotes();
        Task<Note> GetNoteWithContent(string noteID);

        Task<Note> AddNote(string noteContent);
        Task UpdateNote(Note note, string content);
        Task DeleteNote(Note note);
    }
}

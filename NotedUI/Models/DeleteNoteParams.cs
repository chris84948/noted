using NotedUI.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotedUI.Models
{
    public class DeleteNoteParams
    {
        public AllNotesViewModel AllNotes { get; set; }
        public NoteViewModel NoteToDelete { get; set; }

        public DeleteNoteParams(AllNotesViewModel allNotes, NoteViewModel noteToDelete)
        {
            AllNotes = allNotes;
            NoteToDelete = noteToDelete;
        }
    }
}

using NotedUI.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotedUI.Models
{
    public class ListData
    {
        public AllNotesViewModel AllNotes { get; set; }
        public NoteViewModel SelectedNote { get; set; }

        public ListData(AllNotesViewModel allNotes, NoteViewModel selectedNote)
        {
            AllNotes = allNotes;
            SelectedNote = selectedNote;
        }
    }
}

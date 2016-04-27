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
        public ObservableCollection<NoteViewModel> AllNotes { get; set; }
        public NoteViewModel SelectedNote { get; set; }

        public ListData(ObservableCollection<NoteViewModel> allNotes, NoteViewModel selectedNote)
        {
            AllNotes = allNotes;
            SelectedNote = selectedNote;
        }
    }
}

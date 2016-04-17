using NotedUI.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotedUI
{
    public static class ControlExtensions
    {
        public static void AddWithAnimation(this ObservableCollection<NoteViewModel> notes, NoteViewModel newNote)
        {
            newNote.AnimateOnLoad = true;
            notes.Add(newNote);
        }
    }
}

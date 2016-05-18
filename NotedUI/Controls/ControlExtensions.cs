using NotedUI.UI.ViewModels;
using System.Collections.ObjectModel;

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

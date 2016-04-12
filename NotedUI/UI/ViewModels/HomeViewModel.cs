using System;
using JustMVVM;
using System.Collections.ObjectModel;
using NotedUI.UI.Screens;
using NotedUI.Controls;
using System.Collections.Generic;
using NotedUI.Models;
using System.Linq;

namespace NotedUI.UI.ViewModels
{
    public class HomeViewModel : MVVMBase, IScreen
    {
        public event Action<IScreen, eTransitionType> ChangeScreen;

        public FormatCommands Formatting { get; set; }
        public MainCommands MainCommands { get; set; }

        private ObservableCollection<NoteViewModel> _allNotes;
                public ObservableCollection<NoteViewModel> AllNotes
        {
            get { return _allNotes; }
            set
            {
                _allNotes = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<eNoteState> NoteStates
        {
            get
            {
                return Enum.GetValues(typeof(eNoteState))
                    .Cast<eNoteState>();
            }
        }

        public HomeViewModel()
        {
            Formatting = new FormatCommands();
            MainCommands = new MainCommands();

            AllNotes = new ObservableCollection<NoteViewModel>()
            {
                new NoteViewModel("1,", DateTime.Now.Subtract(TimeSpan.FromDays(1)), "Note 1", "Group 1"),
                new NoteViewModel("1,", DateTime.Now.Subtract(TimeSpan.FromMinutes(34)), "Note 2", "Group 1"),
                new NoteViewModel("1,", DateTime.Now, "Note 3", "Group 2"),
                new NoteViewModel("1,", DateTime.Now.Subtract(TimeSpan.FromDays(14)), "Note 4", "Group 2"),
                new NoteViewModel("1,", DateTime.Now.Subtract(TimeSpan.FromHours(7)), "Note 5", "Group 2"),
                new NoteViewModel("1,", DateTime.Now.Subtract(TimeSpan.FromSeconds(35)), "Note 6", "Group 2")
            };
        }

        public void InvokeChangeScreen(IScreen screen)
        {
            ChangeScreen?.Invoke(screen, eTransitionType.SlideInFromLeft);
        }
    }
}

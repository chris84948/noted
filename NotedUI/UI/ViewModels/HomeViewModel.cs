using System;
using JustMVVM;
using System.Collections.ObjectModel;
using NotedUI.UI.Screens;
using NotedUI.Controls;

namespace NotedUI.UI.ViewModels
{
    public class HomeViewModel : MVVMBase, IScreen
    {
        public event Action<IScreen, eTransitionType> ChangeScreen;

        public FormatCommands Formatting { get; set; }
        public MainCommands MainCommands { get; set; }

        private ObservableCollection<Note> _allNotes;
                public ObservableCollection<Note> AllNotes
        {
            get { return _allNotes; }
            set
            {
                _allNotes = value;
                OnPropertyChanged();
            }
        }

        public HomeViewModel()
        {
            Formatting = new FormatCommands();
            MainCommands = new MainCommands();

            AllNotes = new ObservableCollection<Note>()
            {
                new Note() { Group = "GROUP 1", Title = "Note 1", LastModified = DateTime.Now },
                new Note() { Group = "GROUP 1", Title = "Note 2", LastModified = DateTime.Now.Subtract(TimeSpan.FromDays(1)) },
                new Note() { Group = "GROUP 2", Title = "Note 3", LastModified = DateTime.Now.Subtract(TimeSpan.FromMinutes(34)) },
                new Note() { Group = "GROUP 2", Title = "Note 4", LastModified = DateTime.Now },
                new Note() { Group = "GROUP 2", Title = "Note 5", LastModified = DateTime.Now.Subtract(TimeSpan.FromDays(14)) },
                new Note() { Group = "GROUP 2", Title = "Note 6", LastModified = DateTime.Now.Subtract(TimeSpan.FromHours(7)) }
            };
        }

        public void InvokeChangeScreen(IScreen screen)
        {
            ChangeScreen?.Invoke(screen, eTransitionType.SlideInFromLeft);
        }
    }
}

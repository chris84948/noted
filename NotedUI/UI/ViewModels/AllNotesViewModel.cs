using JustMVVM;
using NotedUI.Controls;
using NotedUI.DataStorage;
using NotedUI.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Data;

namespace NotedUI.UI.ViewModels
{
    public class AllNotesViewModel : MVVMBase
    {
        private Timer filterTimer, _updateNoteTimer, _pollAllNotesTimer;

        private AsyncObservableCollection<NoteViewModel> _notes;
        private ICollectionView _view;
        public ICollectionView View
        {
            get { return _view; }
        }

        private NoteViewModel _selectedNote;
        public NoteViewModel SelectedNote
        {
            get { return _selectedNote; }
            set
            {
                _selectedNote = value;
                OnPropertyChanged();
            }
        }

        private string _filter;
        public string Filter
        {
            get { return _filter; }
            set
            {
                _filter = value;
                OnPropertyChanged();

                filterTimer.Stop();
                filterTimer.Start();
            }
        }

        private bool _expandAllGroups;
        public bool ExpandAllGroups
        {
            get { return _expandAllGroups; }
            set
            {
                _expandAllGroups = value;
                OnPropertyChanged();
            }
        }

        internal ILocalStorage LocalStorage { get; set; }

        public AllNotesViewModel()
        {
            _notes = new AsyncObservableCollection<NoteViewModel>();
            LocalStorage = new SQLiteStorage();

            InitializeNotes();

            _view = CollectionViewSource.GetDefaultView(_notes);
            _view.SortDescriptions.Add(new SortDescription("Title", ListSortDirection.Ascending));
            _view.GroupDescriptions.Add(new PropertyGroupDescription("Folder"));
            _view.Filter = NoteFilter;

            //PropertyGroupDescription groupDescription = new PropertyGroupDescription("Group");
            //groupDescription.GroupNames.Add("GROUP 1");
            //groupDescription.GroupNames.Add("GROUP 2");
            //groupDescription.GroupNames.Add("GROUP 3");
            //view.GroupDescriptions.Add(groupDescription);

            filterTimer = new System.Timers.Timer(1000);
            filterTimer.AutoReset = false;
            filterTimer.Elapsed += (object sender, System.Timers.ElapsedEventArgs e) =>
            {
                App.Current.Dispatcher.Invoke(() => _view.Refresh());
                ExpandAllGroups = true;
            };
        }

        private async void InitializeNotes()
        {
            // Get local files first
            await LocalStorage.Initialize();
            _notes = new AsyncObservableCollection<NoteViewModel>((await LocalStorage.GetAllNotes()).Select(x => new NoteViewModel(x)));

            if (_notes.Count > 0)
                SelectedNote = _notes[0];

            // TODO get cloud notes here
            
        }

        private bool NoteFilter(object obj)
        {
            NoteViewModel note = obj as NoteViewModel;

            if (String.IsNullOrEmpty(_filter))
                return true;

            else
                return note.Content.ToUpper().Contains(_filter.ToUpper());
        }
    }
}

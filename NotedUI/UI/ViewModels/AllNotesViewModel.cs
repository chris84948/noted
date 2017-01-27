using ICSharpCode.AvalonEdit;
using JustMVVM;
using NotedUI.Controls;
using NotedUI.DataStorage;
using NotedUI.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Data;
using System.Windows.Input;

namespace NotedUI.UI.ViewModels
{
    public class AllNotesViewModel : MVVMBase
    {
        public ICommand TextChangedCommand { get { return new RelayCommand(TextChangedExec); } }

        private Timer _filterTimer, _updateNoteTimer, _pollAllNotesTimer;

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
                // Update note before changing
                if (_selectedNote != null)
                    UpdateNote(_selectedNote);

                _selectedNote = value;
                OnPropertyChanged();

                if (_selectedNote != null && TextEditor != null)
                    App.Current.Dispatcher.Invoke(() => TextEditor.Text = _selectedNote.Content);
            }
        }

        private GroupCollection _groups;
        public GroupCollection AllGroups
        {
            get { return _groups; }
            set
            {
                _groups = value;
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

                _filterTimer.Stop();
                _filterTimer.Start();
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

        private bool _gettingLatestNotes;
        public bool GettingLatestNotes
        {
            get { return _gettingLatestNotes; }
            set
            {
                _gettingLatestNotes = value;
                OnPropertyChanged();
            }
        }

        private TextEditor _textEditor;
        public TextEditor TextEditor
        {
            get { return _textEditor; }
            set
            {
                _textEditor = value;

                if (_textEditor != null)
                    _textEditor.Text = SelectedNote?.Content ?? "";
            }
        }

        internal ILocalStorage LocalStorage { get; set; }
        internal ICloudStorage CloudStorage { get; set; }

        public AllNotesViewModel()
        {
            _notes = new AsyncObservableCollection<NoteViewModel>();
            LocalStorage = new SQLiteStorage();
            CloudStorage = new GoogleDriveStorage();

            InitializeNotes();

            _filterTimer = new Timer(1000);
            _filterTimer.AutoReset = false;
            _filterTimer.Elapsed += FilterTimer_Elapsed;

            _updateNoteTimer = new Timer(3000);
            _updateNoteTimer.AutoReset = false;
            _updateNoteTimer.Elapsed += (s, e) => UpdateNote(SelectedNote);

            _pollAllNotesTimer = new Timer(60000);
            _pollAllNotesTimer.AutoReset = true;
            _pollAllNotesTimer.Elapsed += async (ss, ee) => await GetAllNotesAndUpdate();
            _pollAllNotesTimer.Start();
        }

        public void AddGroup(GroupViewModel group)
        {
            AllGroups.Add(_view, group);
            CloudStorage.UpdateAllGroups(AllGroups);
        }

        public void UpdateGroup(string oldGroupName, string newGroupName)
        {
            AllGroups.Update(_view, oldGroupName, newGroupName);
            CloudStorage.UpdateAllGroups(AllGroups);
        }

        public void DeleteGroup(string groupName)
        {
            AllGroups.Delete(_view, groupName);
            CloudStorage.UpdateAllGroups(AllGroups);
        }

        private void FilterTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            App.Current.Dispatcher.Invoke(() => _view.Refresh());
            ExpandAllGroups = true;
        }
        
        private async void InitializeNotes()
        {
            // Get local files first
            await LocalStorage.Initialize();
            AllGroups = new GroupCollection((await LocalStorage.GetAllGroups()).Select(x => new GroupViewModel(x)));
            _notes = new AsyncObservableCollection<NoteViewModel>((await LocalStorage.GetAllNotes()).Select(x => new NoteViewModel(x)));

            if (_notes.Count > 0)
                SelectedNote = _notes[0];

            _view = CollectionViewSource.GetDefaultView(_notes);
            _view.SortDescriptions.Add(new SortDescription("Title", ListSortDirection.Ascending));
            AllGroups.UpdateViewGroupDescription(_view);
            _view.Filter = NoteFilter;

            await Task.Run(() => InitializeCloudStorage());
        }

        private async void InitializeCloudStorage()
        {
            await CloudStorage.Connect();

            if (await CloudStorage.DoGroupsNeedToBeUpdated(AllGroups))
                AllGroups = await CloudStorage.GetAllGroups();

            await GetAllNotesAndUpdate();
        }

        private async Task GetAllNotesAndUpdate()
        {
            if (!CloudStorage.IsConnected())
                return;

            GettingLatestNotes = true;

            var notes = await CloudStorage.GetAllNotes();

            string cloudKey = SelectedNote?.CloudKey;

            // Loop through the notes backwards so as to remove them if needed
            for (int i = _notes.Count - 1; i >= 0; i--)
            {
                // Note exists in cloud - update if necessary
                if (notes.ContainsKey(_notes[i]?.CloudKey ?? ""))
                {
                    if (notes[_notes[i].CloudKey].LastModified != _notes[i].LastModified)
                    {
                        await CloudStorage.GetNoteWithContent(_notes[i].NoteData);
                        await LocalStorage.UpdateNote(_notes[i].NoteData);
                    }

                    notes.Remove(_notes[i].CloudKey);
                }
                // Note no longer exists in cloud, delete locally and in localDB
                else if (!notes.ContainsKey(_notes[i]?.CloudKey ?? ""))
                {
                    await LocalStorage.DeleteNote(_notes[i].NoteData);
                    _notes.RemoveAt(i);
                }
            }

            // These are the new notes coming from the cloud
            foreach (Note note in notes.Values)
            {
                await CloudStorage.GetNoteWithContent(note);
                _notes.Add(new NoteViewModel(note));
                await LocalStorage.AddNote(note);
            }

            if (String.IsNullOrEmpty(cloudKey) && _notes.Count > 0)
                SelectedNote = _notes[0];
            else
                SelectedNote = _notes.Where(x => x.CloudKey == cloudKey).FirstOrDefault();

            // Forces a refresh on the converter!
            foreach (var note in _notes)
                note.ForceLastModifiedConverterRefresh();

            GettingLatestNotes = false;
        }

        private async void UpdateNote(NoteViewModel note)
        {
            if (note?.State != eNoteState.Changed)
                return;

            // Make sure to update the note in the cloud first - that's where the update date comes from
            note.State = eNoteState.Syncing;
            await CloudStorage.UpdateNote(note.NoteData);
            await LocalStorage.UpdateNote(note.NoteData);
            note.State = eNoteState.SyncComplete;
        }

        private void TextChangedExec()
        {
            // Update the loccal note storage any time it's updated in the editor
            if (SelectedNote.Content != TextEditor.Text)
                SelectedNote.Content = TextEditor.Text;

            // Reset the update timer every time the text changes
            // This stops it from being updated to the cloud constantly, just after typing has stopped for > 3 seconds
            _updateNoteTimer.Stop();
            _updateNoteTimer.Start();
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

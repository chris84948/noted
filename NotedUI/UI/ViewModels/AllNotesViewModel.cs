using ICSharpCode.AvalonEdit;
using JustMVVM;
using NotedUI.Controls;
using NotedUI.DataStorage;
using NotedUI.Models;
using NotedUI.Utilities;
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
        private HighlightSearchTransformer _highlightSearchTransformer;

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

                bool noteChanging = _selectedNote?.CloudKey != value?.CloudKey;

                _selectedNote = value;
                OnPropertyChanged();

                ChangeNoteAndUpdatePosition(noteChanging);
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

        private bool _offline;
        public bool Offline
        {
            get { return _offline; }
            set
            {
                _offline = value;
                OnPropertyChanged();
            }
        }

        public AllNotesViewModel()
        {
            _notes = new AsyncObservableCollection<NoteViewModel>();

            InitializeNotes();

            _filterTimer = new Timer(1000);
            _filterTimer.AutoReset = false;
            _filterTimer.Elapsed += FilterTimer_Elapsed;

            _updateNoteTimer = new Timer(1500);
            _updateNoteTimer.AutoReset = false;
            _updateNoteTimer.Elapsed += (s, e) => UpdateNote(SelectedNote);

            _pollAllNotesTimer = new Timer(30000);
            _pollAllNotesTimer.AutoReset = true;
            _pollAllNotesTimer.Elapsed += async (ss, ee) => await GetAllNotesAndUpdate();
            _pollAllNotesTimer.Start();

            App.Current.Exit += ApplicationClosing_Exit;
            App.Cloud.InternetConnected += Cloud_InternetConnected;
        }

        private void Cloud_InternetConnected()
        {
            foreach (var note in _notes)
            {
                UpdateNote(note);
            }
        }

        public void Close()
        {
            _filterTimer.Stop();
            _updateNoteTimer.Stop();
            _pollAllNotesTimer.Stop();
            _filterTimer.Dispose();
            _updateNoteTimer.Dispose();
            _pollAllNotesTimer.Dispose();
        }

        private async void ApplicationClosing_Exit(object sender, System.Windows.ExitEventArgs e)
        {
            UpdateNote(SelectedNote);

            await SaveSelectedNote();
        }

        private async Task SaveSelectedNote()
        {
            if (SelectedNote == null)
                return;

            await App.Local.InsertOrUpdateSelectedNoteID(SelectedNote.CloudKey);
        }

        public void AddGroup(GroupViewModel group)
        {
            AllGroups.Add(_view, group);
            App.Cloud.UpdateAllGroups(AllGroups);
        }

        public void UpdateGroup(string oldGroupName, string newGroupName)
        {
            AllGroups.Update(_view, oldGroupName, newGroupName);
            App.Cloud.UpdateAllGroups(AllGroups);
        }

        public void DeleteGroup(string groupName)
        {
            AllGroups.Delete(_view, groupName);
            App.Cloud.UpdateAllGroups(AllGroups);
        }

        private void FilterTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            App.Current.Dispatcher.Invoke(() => _view.Refresh());
            ExpandAllGroups = true;

            UpdateSearchTextHighlighting();
        }

        private void UpdateSearchTextHighlighting()
        {
            if (_highlightSearchTransformer != null)
            {
                App.Current.Dispatcher.Invoke(() => TextEditor.TextArea.TextView.LineTransformers.Remove(_highlightSearchTransformer));
                _highlightSearchTransformer = null;
            }

            if (!String.IsNullOrEmpty(Filter))
            {
                _highlightSearchTransformer = new HighlightSearchTransformer(Filter);
                App.Current.Dispatcher.Invoke(() => TextEditor.TextArea.TextView.LineTransformers.Add(_highlightSearchTransformer));
            }
        }
        
        private async void InitializeNotes()
        {
            // Get local files first
            await App.Local.Initialize();
            AllGroups = new GroupCollection((await App.Local.GetAllGroups()).Select(x => new GroupViewModel(x)));
            _notes = new AsyncObservableCollection<NoteViewModel>((await App.Local.GetAllNotes()).Select(x => new NoteViewModel(x)));

            string storedCloudKey = await App.Local.GetSelectedNoteID();
            if (!string.IsNullOrEmpty(storedCloudKey))
                SelectedNote = _notes.Where(n => n.CloudKey == storedCloudKey).FirstOrDefault();

            _view = CollectionViewSource.GetDefaultView(_notes);
            _view.SortDescriptions.Add(new SortDescription("Title", ListSortDirection.Ascending));
            AllGroups.UpdateViewGroupDescription(_view);
            _view.Filter = NoteFilter;

            await Task.Run(() => CheckForCloudUpdates());
        }

        private async void CheckForCloudUpdates()
        {
            // TODO turn this on when ready
            Updater.CheckForUpdates(App.Cloud);

            if (await App.Cloud.DoGroupsNeedToBeUpdated(AllGroups))
                AllGroups = await App.Cloud.GetAllGroups();

            await GetAllNotesAndUpdate();
        }

        private async Task GetAllNotesAndUpdate()
        {
            Offline = !App.Cloud.IsConnected();
            if (Offline)
                return;

            GettingLatestNotes = true;

            var notes = await App.Cloud.GetAllNotes();
            if (notes == null) // null if we time out trying to connect
                return;

            string cloudKey = SelectedNote?.CloudKey;

            // Loop through the notes backwards so as to remove them if needed
            for (int i = _notes.Count - 1; i >= 0; i--)
            {
                // Note exists in cloud - update if necessary
                if (notes.ContainsKey(_notes[i]?.CloudKey ?? ""))
                {
                    if (notes[_notes[i].CloudKey].LastModified > _notes[i].LastModified) // Cloud note is newer
                    {
                        await App.Cloud.GetNoteWithContent(_notes[i].NoteData);
                        await App.Local.UpdateNote(_notes[i].NoteData);
                    }
                    else if (notes[_notes[i].CloudKey].LastModified < _notes[i].LastModified) // Local note is newer
                    {
                        await App.Cloud.UpdateNote(_notes[i].NoteData);
                    }

                    notes.Remove(_notes[i].CloudKey);
                }
                // Note no longer exists in cloud, delete locally and in localDB
                else if (!notes.ContainsKey(_notes[i]?.CloudKey ?? ""))
                {
                    await App.Local.DeleteNote(_notes[i].NoteData);
                    _notes.RemoveAt(i);
                }
            }

            // These are the new notes coming from the cloud
            foreach (Note note in notes.Values)
            {
                await App.Cloud.GetNoteWithContent(note);
                _notes.Add(new NoteViewModel(note));
                await App.Local.AddNote(note);
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
            await App.Cloud.UpdateNote(note.NoteData);

            // Only update last modified date when the cloud isn't connected.
            // If the internet is there, we'll get the last modified date from the cloud file
            if (!App.Cloud.IsConnected())
                note.LastModified = DateTime.Now;

            await App.Local.UpdateNote(note.NoteData);
            note.State = eNoteState.SyncComplete;
        }

        private void TextChangedExec()
        {
            // Update the loccal note storage any time it's updated in the editor
            if (SelectedNote.Content != TextEditor.Text)
                SelectedNote.Content = TextEditor.Text;

            // Reset the update timer every time the text changes
            // This stops it from being updated to the cloud constantly, just after typing has stopped for update time
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

        private void ChangeNoteAndUpdatePosition(bool changingNote)
        {
            if (_selectedNote != null && TextEditor != null)
            {
                App.Current.Dispatcher.Invoke(() =>
                {
                    int offset = TextEditor.CaretOffset;
                    TextEditor.Text = _selectedNote.Content;
                    TextEditor.CaretOffset = changingNote ? 0 : offset;
                });
            }
        }
    }
}

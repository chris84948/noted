using JustMVVM;
using NotedUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotedUI.UI.ViewModels
{
    public class NoteViewModel : MVVMBase
    {
        /// <summary>
        /// Underlying object for writing/reading from SQL
        /// </summary>
        internal Note NoteData { get; set; } = new Note();

        public long ID
        {
            get { return NoteData.ID; }
            set
            {
                NoteData.ID = value;
                OnPropertyChanged();
            }
        }

        public string CloudKey
        {
            get { return NoteData.CloudKey; }
            set
            {
                NoteData.CloudKey = value;
                OnPropertyChanged();
            }
        }

        public DateTime? LastModified
        {
            get { return NoteData.LastModified; }
            set
            {
                NoteData.LastModified = value;
                OnPropertyChanged();
            }
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        public string Content
        {
            get { return NoteData.Content; }
            set
            {
                NoteData.Content = value;
                OnPropertyChanged();

                Title = GetTitle();
                SetNoteState();
            }
        }

        public string Folder
        {
            get { return NoteData.Folder; }
            set
            {
                NoteData.Folder = value;
                OnPropertyChanged();
            }
        }

        private eNoteState _state;
        public eNoteState State
        {
            get { return _state; }
            set
            {
                _state = value;
                OnPropertyChanged();
            }
        }

        public bool AnimateOnLoad { get; set; } = false;

        public NoteViewModel(long id,
                             DateTime? lastModified,
                             string content,
                             string folder,
                             string cloudKey = null)
        {
            ID = id;
            LastModified = lastModified;
            Content = content;
            Folder = folder;
            CloudKey = cloudKey;

            State = eNoteState.Normal;
        }

        public NoteViewModel(Note note)
            : this(note.ID, note.LastModified, note.Content, note.Folder, note.CloudKey)
        { }

        private string GetTitle()
        {
            int index = NoteData.Content.IndexOf("\r\n");
            
            if (index == -1)
                return NoteData.Content;
            else
                return NoteData.Content.Substring(0, index);
        }

        private void SetNoteState()
        {
            // Only change the note if it's in a Normal state
            if (State == eNoteState.Normal)
                State = eNoteState.Changed;
        }
    }
}

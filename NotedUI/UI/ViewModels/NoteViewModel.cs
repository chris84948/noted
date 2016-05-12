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

        private bool _isMarkedForRemoval;
        public bool IsMarkedForRemoval
        {
            get { return _isMarkedForRemoval; }
            set
            {
                _isMarkedForRemoval = value;
                OnPropertyChanged();
            }
        }

        public bool AnimateOnLoad { get; set; } = false;

        public NoteViewModel(int id,
                             DateTime? lastModified,
                             string content,
                             string folder)
        {
            ID = id;
            LastModified = lastModified;
            Content = content;
            Folder = folder;

            State = eNoteState.Normal;
            IsMarkedForRemoval = false;
        }

        public NoteViewModel(Note note)
        {
            ID = note.ID;
            LastModified = note.LastModified;
            Content = note.Content;
            Folder = note.Folder;
            CloudKey = note.CloudKey;
        }

        private string GetTitle()
        {
            int index = NoteData.Content.IndexOf("\r\n");
            
            if (index == -1)
                return NoteData.Content;
            else
                return NoteData.Content.Substring(0, index);
        }
    }
}

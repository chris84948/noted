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
        private Note _note = new Note();

        public long ID
        {
            get { return _note.ID; }
            set
            {
                _note.ID = value;
                OnPropertyChanged();
            }
        }

        public string CloudKey
        {
            get { return _note.CloudKey; }
            set
            {
                _note.CloudKey = value;
                OnPropertyChanged();
            }
        }

        public DateTime? LastModified
        {
            get { return _note.LastModified; }
            set
            {
                _note.LastModified = value;
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
            get { return _note.Content; }
            set
            {
                _note.Content = value;
                OnPropertyChanged();

                Title = GetTitle();
            }
        }

        public string Folder
        {
            get { return _note.Folder; }
            set
            {
                _note.Folder = value;
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
            int index = _note.Content.IndexOf("\r\n");

            if (index == -1 && _note.Content.Length > 20 || index > 20)
                return _note.Content.Substring(0, 20);
            else if (index == -1)
                return _note.Content;
            else
                return _note.Content.Substring(0, index);
        }
    }
}

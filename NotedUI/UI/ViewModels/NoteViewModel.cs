using JustMVVM;
using NotedUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotedUI.UI.ViewModels
{
    class NoteViewModel : MVVMBase
    {
        private Note _note;

        public string ID
        {
            get { return _note.ID; }
            set
            {
                _note.ID = value;
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

        public string Content
        {
            get { return _note.Content; }
            set
            {
                _note.Content = value;
                OnPropertyChanged();
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

        public NoteViewModel(string id,
                             DateTime? lastModified,
                             string content,
                             string folder)
        {
            ID = id;
            LastModified = lastModified;
            Content = content;
            Folder = folder;

            State = eNoteState.Normal;
        }
    }
}

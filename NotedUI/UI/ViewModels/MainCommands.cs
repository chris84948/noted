using JustMVVM;
using NotedUI.UI.Screens;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NotedUI.UI.ViewModels
{
    class MainCommands : MVVMBase
    {
        public ICommand AddNoteCommand { get { return new RelayCommand<ObservableCollection<Note>>(AddNoteExec, CanAddNoteExec); } }
        public ICommand DeleteNoteCommand { get { return new RelayCommand<ObservableCollection<Note>>(DeleteNoteExec, CanDeleteNoteExec); } }
        public ICommand FolderAddCommand { get { return new RelayCommand<ObservableCollection<Note>>(FolderAddExec, CanFolderAddExec); } }
        public ICommand FolderDeleteCommand { get { return new RelayCommand<ObservableCollection<Note>>(FolderDeleteExec, CanFolderDeleteExec); } }
        public ICommand ExportHTMLCommand { get { return new RelayCommand<ObservableCollection<Note>>(ExportHTMLExec, CanExportHTMLExec); } }
        public ICommand ExportPDFCommand { get { return new RelayCommand<ObservableCollection<Note>>(ExportPDFExec, CanExportPDFExec); } }
        public ICommand ShowSettingsCommand { get { return new RelayCommand<ObservableCollection<Note>>(ShowSettingsExec, CanShowSettingsExec); } }

        public bool CanAddNoteExec(ObservableCollection<Note> AllNotes)
        {
            return true;
        }

        public void AddNoteExec(ObservableCollection<Note> AllNotes)
        {
            AllNotes.Add(new Note() { Group = "GROUP 1", Title = "Note 10", LastModified = DateTime.Now });
        }

        public bool CanDeleteNoteExec(ObservableCollection<Note> AllNotes)
        {
            return true;
        }

        public void DeleteNoteExec(ObservableCollection<Note> AllNotes)
        {

        }

        public bool CanFolderAddExec(ObservableCollection<Note> AllNotes)
        {
            return true;
        }

        public void FolderAddExec(ObservableCollection<Note> AllNotes)
        {

        }

        public bool CanFolderDeleteExec(ObservableCollection<Note> AllNotes)
        {
            return true;
        }

        public void FolderDeleteExec(ObservableCollection<Note> AllNotes)
        {

        }

        public bool CanExportHTMLExec(ObservableCollection<Note> AllNotes)
        {
            return true;
        }

        public void ExportHTMLExec(ObservableCollection<Note> AllNotes)
        {

        }

        public bool CanExportPDFExec(ObservableCollection<Note> AllNotes)
        {
            return true;
        }

        public void ExportPDFExec(ObservableCollection<Note> AllNotes)
        {

        }

        public bool CanShowSettingsExec(ObservableCollection<Note> AllNotes)
        {
            return true;
        }

        public void ShowSettingsExec(ObservableCollection<Note> AllNotes)
        {

        }
    }
}

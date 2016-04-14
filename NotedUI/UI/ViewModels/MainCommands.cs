using JustMVVM;
using NotedUI.Models;
using NotedUI.UI.Screens;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace NotedUI.UI.ViewModels
{
    public class MainCommands : MVVMBase
    {
        public ICommand AddNoteCommand { get { return new RelayCommand<ObservableCollection<NoteViewModel>>(AddNoteExec, CanAddNoteExec); } }
        public ICommand PrepareToDeleteCommand { get { return new RelayCommand<ListData>(PrepareToDeleteExec, CanPrepareToDelete); } }
        public ICommand DeleteNoteCommand { get { return new RelayCommand<ListData>(DeleteNoteExec, CanDeleteNoteExec); } }
        public ICommand FolderAddCommand { get { return new RelayCommand<ObservableCollection<NoteViewModel>>(FolderAddExec, CanFolderAddExec); } }
        public ICommand FolderDeleteCommand { get { return new RelayCommand<ObservableCollection<NoteViewModel>>(FolderDeleteExec, CanFolderDeleteExec); } }
        public ICommand ExportHTMLCommand { get { return new RelayCommand<ObservableCollection<NoteViewModel>>(ExportHTMLExec, CanExportHTMLExec); } }
        public ICommand ExportPDFCommand { get { return new RelayCommand<ObservableCollection<NoteViewModel>>(ExportPDFExec, CanExportPDFExec); } }

        public bool CanAddNoteExec(ObservableCollection<NoteViewModel> allNotes)
        {
            return true;
        }

        public void AddNoteExec(ObservableCollection<NoteViewModel> allNotes)
        {
            allNotes.Add(new NoteViewModel("1", DateTime.Now, "Note 10", "Group 1"));
        }

        public bool CanPrepareToDelete(ListData data)
        {
            return data.SelectedNote != null;
        }

        public void PrepareToDeleteExec(ListData data)
        {
            data.SelectedNote.IsMarkedForRemoval = true;
        }

        public bool CanDeleteNoteExec(ListData data)
        {
            return data.SelectedNote != null;
        }

        public void DeleteNoteExec(ListData data)
        {
            data.AllNotes.Remove(data.SelectedNote);
        }

        public bool CanFolderAddExec(ObservableCollection<NoteViewModel> allNotes)
        {
            return true;
        }

        public void FolderAddExec(ObservableCollection<NoteViewModel> allNotes)
        {

        }

        public bool CanFolderDeleteExec(ObservableCollection<NoteViewModel> allNotes)
        {
            return true;
        }

        public void FolderDeleteExec(ObservableCollection<NoteViewModel> allNotes)
        {

        }

        public bool CanExportHTMLExec(ObservableCollection<NoteViewModel> allNotes)
        {
            return true;
        }

        public void ExportHTMLExec(ObservableCollection<NoteViewModel> allNotes)
        {

        }

        public bool CanExportPDFExec(ObservableCollection<NoteViewModel> allNotes)
        {
            return true;
        }

        public void ExportPDFExec(ObservableCollection<NoteViewModel> allNotes)
        {

        }
    }
}

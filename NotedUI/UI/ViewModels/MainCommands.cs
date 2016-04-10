using JustMVVM;
using NotedUI.UI.Screens;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace NotedUI.UI.ViewModels
{
    public class MainCommands : MVVMBase
    {
        public ICommand AddNoteCommand { get { return new RelayCommand<ObservableCollection<Note>>(AddNoteExec, CanAddNoteExec); } }
        public ICommand PrepareToDeleteCommand { get { return new RelayCommand<ListData>(PrepareToDeleteExec, CanPrepareToDelete); } }
        public ICommand DeleteNoteCommand { get { return new RelayCommand<ListData>(DeleteNoteExec, CanDeleteNoteExec); } }
        public ICommand FolderAddCommand { get { return new RelayCommand<ObservableCollection<Note>>(FolderAddExec, CanFolderAddExec); } }
        public ICommand FolderDeleteCommand { get { return new RelayCommand<ObservableCollection<Note>>(FolderDeleteExec, CanFolderDeleteExec); } }
        public ICommand ExportHTMLCommand { get { return new RelayCommand<ObservableCollection<Note>>(ExportHTMLExec, CanExportHTMLExec); } }
        public ICommand ExportPDFCommand { get { return new RelayCommand<ObservableCollection<Note>>(ExportPDFExec, CanExportPDFExec); } }
        public ICommand ShowSettingsCommand { get { return new RelayCommand<HomeViewModel>(ShowSettingsExec, CanShowSettingsExec); } }

        public bool CanAddNoteExec(ObservableCollection<Note> allNotes)
        {
            return true;
        }

        public void AddNoteExec(ObservableCollection<Note> allNotes)
        {
            allNotes.Add(new Note() { Group = "GROUP 1", Title = "Note 10", LastModified = DateTime.Now });
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

        public bool CanFolderAddExec(ObservableCollection<Note> allNotes)
        {
            return true;
        }

        public void FolderAddExec(ObservableCollection<Note> allNotes)
        {

        }

        public bool CanFolderDeleteExec(ObservableCollection<Note> allNotes)
        {
            return true;
        }

        public void FolderDeleteExec(ObservableCollection<Note> allNotes)
        {

        }

        public bool CanExportHTMLExec(ObservableCollection<Note> allNotes)
        {
            return true;
        }

        public void ExportHTMLExec(ObservableCollection<Note> allNotes)
        {

        }

        public bool CanExportPDFExec(ObservableCollection<Note> allNotes)
        {
            return true;
        }

        public void ExportPDFExec(ObservableCollection<Note> allNotes)
        {

        }

        public bool CanShowSettingsExec(HomeViewModel homeVM)
        {
            return true;
        }

        public void ShowSettingsExec(HomeViewModel homeVM)
        {
            var settings = new SettingsViewModel(homeVM);
            homeVM.InvokeChangeScreen(settings);
        }
    }
}

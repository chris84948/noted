using JustMVVM;
using NotedUI.Models;
using NotedUI.UI.Components;
using NotedUI.UI.Screens;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace NotedUI.UI.ViewModels
{
    public class MainCommands : MVVMBase
    {
        public ICommand AddNoteCommand { get { return new RelayCommand<AllNotesViewModel>(AddNoteExec, CanAddNoteExec); } }
        public ICommand PrepareToDeleteCommand { get { return new RelayCommand<ListData>(PrepareToDeleteExec, CanPrepareToDelete); } }
        public ICommand DeleteNoteCommand { get { return new RelayCommand<ListData>(DeleteNoteExec, CanDeleteNoteExec); } }
        public ICommand FolderAddCommand { get { return new RelayCommand<AllNotesViewModel>(FolderAddExec, CanFolderAddExec); } }
        public ICommand FolderDeleteCommand { get { return new RelayCommand<AllNotesViewModel>(FolderDeleteExec, CanFolderDeleteExec); } }
        public ICommand ExportHTMLCommand { get { return new RelayCommand<AllNotesViewModel>(ExportHTMLExec, CanExportHTMLExec); } }
        public ICommand ExportPDFCommand { get { return new RelayCommand<AllNotesViewModel>(ExportPDFExec, CanExportPDFExec); } }
        public ICommand ShowSettingsCommand { get { return new RelayCommand<HomeViewModel>(ShowSettingsExec, CanShowSettingsExec); } }

        public ICommand ToggleFormattingCommand { get { return new RelayCommand<MainMenu>(ToggleFormattingExec); } }
        public ICommand TogglePreviewCommand { get { return new RelayCommand<MainMenu>(TogglePreviewExec); } }

        private bool _showPreview;
        public bool ShowPreview
        {
            get { return _showPreview; }
            set
            {
                _showPreview = value;
                OnPropertyChanged();
            }
        }

        public bool CanAddNoteExec(AllNotesViewModel allNotes)
        {
            return true;
        }

        public void AddNoteExec(AllNotesViewModel allNotes)
        {
            (allNotes.View.SourceCollection as ObservableCollection<NoteViewModel>).
                AddWithAnimation(new NoteViewModel("1", DateTime.Now, "Note 10", "Group 1"));
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

        public bool CanFolderAddExec(AllNotesViewModel allNotes)
        {
            return true;
        }

        public void FolderAddExec(AllNotesViewModel allNotes)
        {

        }

        public bool CanFolderDeleteExec(AllNotesViewModel allNotes)
        {
            return true;
        }

        public void FolderDeleteExec(AllNotesViewModel allNotes)
        {

        }

        public bool CanExportHTMLExec(AllNotesViewModel allNotes)
        {
            return true;
        }

        public void ExportHTMLExec(AllNotesViewModel allNotes)
        {

        }

        public bool CanExportPDFExec(AllNotesViewModel allNotes)
        {
            return true;
        }

        public void ExportPDFExec(AllNotesViewModel allNotes)
        {

        }

        public bool CanShowSettingsExec(HomeViewModel homeVM)
        {
            return true;
        }

        public void ShowSettingsExec(HomeViewModel homeVM)
        {
            if (ShowPreview)
            {
                ShowPreview = false;
                homeVM.ShowPreviewOnLoad = true;
            }

            var settings = new SettingsViewModel(homeVM);
            homeVM.InvokeChangeScreen(settings);
        }

        private void ToggleFormattingExec(MainMenu menu)
        {
            menu.ShowFormatMenu = !menu.ShowFormatMenu;
        }

        private void TogglePreviewExec(MainMenu menu)
        {
            menu.ShowPreview = !menu.ShowPreview;
        }
    }
}

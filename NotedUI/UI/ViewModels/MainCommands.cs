﻿using CommonMark;
using ICSharpCode.AvalonEdit;
using JustMVVM;
using NotedUI.Export;
using NotedUI.Models;
using NotedUI.UI.Components;
using NotedUI.UI.Screens;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace NotedUI.UI.ViewModels
{
    public class MainCommands : MVVMBase
    {
        public ICommand AddNoteCommand { get { return new RelayCommand<AllNotesViewModel>(AddNoteExec, CanAddNoteExec); } }
        public ICommand DeleteNoteCommand { get { return new RelayCommand<AllNotesViewModel>(DeleteNoteExec, CanDeleteNoteExec); } }
        public ICommand FolderAddCommand { get { return new RelayCommand<AllNotesViewModel>(FolderAddExec, CanFolderAddExec); } }
        public ICommand FolderDeleteCommand { get { return new RelayCommand<AllNotesViewModel>(FolderDeleteExec, CanFolderDeleteExec); } }
        public ICommand ExportHTMLCommand { get { return new RelayCommand<AllNotesViewModel>(ExportHTMLExec, CanExportHTMLExec); } }
        public ICommand ExportPDFCommand { get { return new RelayCommand<AllNotesViewModel>(ExportPDFExec, CanExportPDFExec); } }
        public ICommand ShowSettingsCommand { get { return new RelayCommand<HomeViewModel>(ShowSettingsExec, CanShowSettingsExec); } }

        public ICommand ToggleFormattingCommand { get { return new RelayCommand<MainMenu>(ToggleFormattingExec); } }
        public ICommand TogglePreviewCommand { get { return new RelayCommand<MainMenu>(TogglePreviewExec); } }
        public ICommand ToggleSearchCommand { get { return new RelayCommand<MainMenu>(ToggleSearchExec); } }

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

        public bool CanNotedExec(AllNotesViewModel allNotes)
        {
            return true;
        }

        public void NotedExec(AllNotesViewModel allNotes)
        {
            
        }

        public bool CanAddNoteExec(AllNotesViewModel allNotes)
        {
            return true;
        }

        public async void AddNoteExec(AllNotesViewModel allNotes)
        {
            string folderName = "Group 1";
            int id = (int)await allNotes.LocalStorage.AddNote(folderName);

            var newNote = new NoteViewModel(id, DateTime.Now, "", folderName);
            (allNotes.View.SourceCollection as ObservableCollection<NoteViewModel>).Add(newNote);
            allNotes.SelectedNote = newNote;
        }

        public bool CanDeleteNoteExec(AllNotesViewModel allNotes)
        {
            return allNotes.SelectedNote != null;
        }

        public void DeleteNoteExec(AllNotesViewModel allNotes)
        {
            var noteList = allNotes.View.SourceCollection as ObservableCollection<NoteViewModel>;
            int noteIndex = noteList.IndexOf(allNotes.SelectedNote);

            allNotes.LocalStorage.DeleteNote(allNotes.SelectedNote.NoteData);
            (allNotes.View.SourceCollection as ObservableCollection<NoteViewModel>).Remove(allNotes.SelectedNote);

            if (noteList.Count == 0)
                return;

            allNotes.SelectedNote = noteList[noteIndex < noteList.Count ? noteIndex : noteIndex - 1];
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
            //var html = CommonMarkConverter.Convert(_markdown);

            //HTMLExporter.Export(@"c:\github\notedui\htmltestexport.html", "github", html);
        }

        public bool CanExportPDFExec(AllNotesViewModel allNotes)
        {
            return true;
        }

        public void ExportPDFExec(AllNotesViewModel allNotes)
        {
            //var html = CommonMarkConverter.Convert(_markdown);

            //PDFExporter.Export(@"c:\github\notedui\pdftestexport.pdf", "github", html);
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

        private void ToggleSearchExec(MainMenu menu)
        {
            menu.ShowSearch = !menu.ShowSearch;
        }
    }
}

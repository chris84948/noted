using CommonMark;
using JustMVVM;
using NotedUI.Export;
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
        // TODO eventually link this to the actual note content
        string _markdown = @"
## Welcome to MarkdownPad 2 ##

**MarkdownPad** is a full-featured Markdown editor for Windows.

### Built exclusively for Markdown ###

Enjoy first-class Markdown support with easy access to  Markdown syntax and convenient keyboard shortcuts.

Give them a try:

- **Bold** (`Ctrl+B`) and *Italic* (`Ctrl+I`)
- Quotes (`Ctrl+Q`)
- Code blocks (`Ctrl+K`)
- Headings 1, 2, 3 (`Ctrl+1`, `Ctrl+2`, `Ctrl+3`)
- Lists (`Ctrl+U` and `Ctrl+Shift+O`)

### See your changes instantly with LivePreview ###

Don't guess if your [hyperlink syntax](http://markdownpad.com) is correct; LivePreview will show you exactly what your document looks like every time you press a key.

### Make it your own ###

Fonts, color schemes, layouts and stylesheets are all 100% customizable so you can turn MarkdownPad into your perfect editor.

### A robust editor for advanced Markdown users ###

MarkdownPad supports multiple Markdown processing engines, including standard Markdown, Markdown Extra (with Table support) and GitHub Flavored Markdown.

With a tabbed document interface, PDF export, a built-in image uploader, session management, spell check, auto-save, syntax highlighting and a built-in CSS management interface, there's no limit to what you can do with MarkdownPad.";



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
            (allNotes.View.SourceCollection as ObservableCollection<NoteViewModel>).
                AddWithAnimation(new NoteViewModel("1", DateTime.Now, "Note 10", "Group 1"));
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
            var html = CommonMarkConverter.Convert(_markdown);

            HTMLExporter.Export(@"c:\github\notedui\htmltestexport.html", "github", html);
        }

        public bool CanExportPDFExec(AllNotesViewModel allNotes)
        {
            return true;
        }

        public void ExportPDFExec(AllNotesViewModel allNotes)
        {
            var html = CommonMarkConverter.Convert(_markdown);

            PDFExporter.Export(@"c:\github\notedui\pdftestexport.pdf", "github", html);
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

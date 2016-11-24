using CommonMark;
using JustMVVM;
using NotedUI.Export;
using NotedUI.Models;
using NotedUI.UI.Components;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NotedUI.UI.ViewModels
{
    public class MainCommands : MVVMBase
    {
        public ICommand AddNoteCommand { get { return new RelayCommand<AddNoteParams>(AddNoteExec, CanAddNoteExec); } }
        public ICommand DeleteNoteCommand { get { return new RelayCommand<DeleteNoteParams>(DeleteNoteExec, CanDeleteNoteExec); } }

        public ICommand AddGroupCommand { get { return new RelayCommand<GroupCmdParams>(AddGroupExec, CanAddGroupExec); } }
        public ICommand RenameGroupCommand { get { return new RelayCommand<GroupCmdParams>(RenameGroupExec); } }
        public ICommand DeleteGroupCommand { get { return new RelayCommand<GroupCmdParams>(DeleteGroupExec, CanDeleteGroupExec); } }

        public ICommand ExportTextCommand { get { return new RelayCommand<AllNotesViewModel>(ExportTextExec, CanExportTextExec); } }
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

        public bool CanAddNoteExec(AddNoteParams cmdArgs)
        {
            return true;
        }

        public async void AddNoteExec(AddNoteParams cmdArgs)
        {
            // Little complicated here
            // First add the note to local storage to get the ID
            int id = (int)await cmdArgs.AllNotes.LocalStorage.AddNote(cmdArgs.GroupName);
            var newNote = new NoteViewModel(id, DateTime.Now, "", cmdArgs.GroupName);

            // Add the note to the screen now so there's no delay
            (cmdArgs.AllNotes.View.SourceCollection as ObservableCollection<NoteViewModel>).Add(newNote);
            cmdArgs.AllNotes.SelectedNote = newNote;

            await Task.Run(async () =>
            {
                // Finally add to the cloud, update the local DB with the cloud key and modified date and update the SelectedNote
                await cmdArgs.AllNotes.CloudStorage.AddNote(cmdArgs.AllNotes.SelectedNote.NoteData);
                await cmdArgs.AllNotes.LocalStorage.UpdateNote(cmdArgs.AllNotes.SelectedNote.NoteData);
            });
        }

        public bool CanDeleteNoteExec(DeleteNoteParams cmdArgs)
        {
            return true;
        }

        public async void DeleteNoteExec(DeleteNoteParams cmdArgs)
        {
            var noteList = cmdArgs.AllNotes.View.SourceCollection as ObservableCollection<NoteViewModel>;
            noteList.Remove(cmdArgs.NoteToDelete);

            await cmdArgs.AllNotes.LocalStorage.DeleteNote(cmdArgs.NoteToDelete.NoteData);
            await cmdArgs.AllNotes.CloudStorage.DeleteNote(cmdArgs.NoteToDelete.NoteData);

            if (noteList.Count == 0)
                return;

            // TODO For now, just unselect all notes when it's deleted
            cmdArgs.AllNotes.SelectedNote = null;
        }

        public bool CanAddGroupExec(GroupCmdParams cmdArgs)
        {
            return true;
        }

        public void AddGroupExec(GroupCmdParams cmdArgs)
        {
            var dialog = new GroupNameDialogViewModel(cmdArgs.AllNotes.AllGroups, String.Empty, "Add New Group");

            dialog.DialogClosed += async () =>
            {
                await Task.Delay(300);
                cmdArgs.HomeVM.FixAirspace = false;

                if (dialog.Result == System.Windows.Forms.DialogResult.Cancel)
                    return;

                // Get ID, add group to local storage
                var id = await cmdArgs.AllNotes.LocalStorage.AddGroup(dialog.GroupName);
                cmdArgs.AllNotes.AddGroup(new GroupViewModel(id, dialog.GroupName));
            };

            cmdArgs.HomeVM.InvokeShowDialog(dialog);
        }

        private void RenameGroupExec(GroupCmdParams cmdArgs)
        {
            var dialog = new GroupNameDialogViewModel(cmdArgs.AllNotes.AllGroups, cmdArgs.GroupName, "Rename Group");

            dialog.DialogClosed += async () =>
            {
                await Task.Delay(300);
                cmdArgs.HomeVM.FixAirspace = false;

                if (dialog.Result == System.Windows.Forms.DialogResult.Cancel)
                    return;

                // Update the group name in the DB then update the notes in the VM
                await cmdArgs.AllNotes.LocalStorage.UpdateGroup(cmdArgs.GroupName, dialog.GroupName);

                // Update the notes with this group name
                foreach (var note in (cmdArgs.AllNotes.View.SourceCollection as ObservableCollection<NoteViewModel>))
                {
                    string oldGroupName = cmdArgs.GroupName.ToUpper();
                    if (note.Group.ToUpper() == oldGroupName)
                        note.Group = dialog.GroupName;
                }

                // Update the group name
                cmdArgs.AllNotes.UpdateGroup(cmdArgs.GroupName, dialog.GroupName);
            };

            cmdArgs.HomeVM.InvokeShowDialog(dialog);
        }

        private bool CanDeleteGroupExec(GroupCmdParams cmdArgs)
        {
            if (cmdArgs == null)
                return false;

            var notes = (cmdArgs?.AllNotes?.View?.SourceCollection
                            as ObservableCollection<NoteViewModel>).ToList()
                                                                   .Where(x => x?.Group?.ToUpper() == cmdArgs?.GroupName?.ToUpper());
            // Can only delete a group if it's empty!
            return notes?.Count() == 0;
        }

        private async void DeleteGroupExec(GroupCmdParams cmdArgs)
        {
            await cmdArgs.AllNotes.LocalStorage.DeleteGroup(cmdArgs.GroupName);

            cmdArgs.AllNotes.DeleteGroup(cmdArgs.GroupName);
        }

        public bool CanExportTextExec(AllNotesViewModel allNotes)
        {
            return true;
        }

        public void ExportTextExec(AllNotesViewModel allNotes)
        {
            TextExporter.Export(@"c:\github\notedui\testExport.txt", allNotes.SelectedNote.Content);
        }

        public bool CanExportHTMLExec(AllNotesViewModel allNotes)
        {
            return true;
        }

        public void ExportHTMLExec(AllNotesViewModel allNotes)
        {
            var html = CommonMarkConverter.Convert(allNotes.SelectedNote.Content);

            HTMLExporter.Export(@"c:\github\notedui\textExport.html", "github", html);
        }

        public bool CanExportPDFExec(AllNotesViewModel allNotes)
        {
            return true;
        }

        public void ExportPDFExec(AllNotesViewModel allNotes)
        {
            var html = CommonMarkConverter.Convert(allNotes.SelectedNote.Content);

            PDFExporter.Export(@"c:\github\notedui\textExport.pdf", "github", html);
        }

        public bool CanShowSettingsExec(HomeViewModel homeVM)
        {
            return true;
        }

        public void ShowSettingsExec(HomeViewModel homeVM)
        {
            if (homeVM.MainCommands.ShowPreview)
                homeVM.FixAirspace = true;

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

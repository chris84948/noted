using System;
using JustMVVM;
using NotedUI.Controls;
using System.Collections.Generic;
using NotedUI.Models;
using System.Linq;
using GongSolutions.Wpf.DragDrop;
using System.Windows;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using NotedUI.Utilities;

namespace NotedUI.UI.ViewModels
{
    public class HomeViewModel : MVVMBase, IScreen, IDropTarget
    {
        public event Action<IScreen, eTransitionType> ChangeScreen;
        public event Action<IDialog> ShowDialog;

        public FormatCommands Formatting { get; set; }
        public MainCommands MainCommands { get; set; }
        public AllNotesViewModel AllNotes { get; set; }
        public string CSSStyle { get; set; }

        private bool _fixAirspace;
        public bool FixAirspace
        {
            get { return _fixAirspace; }
            set
            {
                _fixAirspace = value;
                OnPropertyChanged();
            }
        }

        public HomeViewModel()
        {
            AllNotes = new AllNotesViewModel();
            Formatting = new FormatCommands(this, AllNotes.TextEditor);
            MainCommands = new MainCommands(this, AllNotes, AllNotes.LocalStorage);
            CSSStyle = File.ReadAllText("Resources\\CSS\\Github.css");
        }

        public void InvokeChangeScreen(IScreen screen)
        {
            ChangeScreen?.Invoke(screen, eTransitionType.SlideInFromLeft);
        }

        public void InvokeShowDialog(IDialog dialog)
        {
            FixAirspace = true;
            dialog.DialogClosed += async () =>
            {
                await Task.Delay(300);
                FixAirspace = false;
            };

            ShowDialog?.Invoke(dialog);
        }

        public void DragOver(IDropInfo dropInfo)
        {
            NoteViewModel sourceItem = dropInfo.Data as NoteViewModel;

            if (sourceItem != null && dropInfo.TargetGroup != null)
            {
                dropInfo.Effects = DragDropEffects.Move;
            }
        }

        void IDropTarget.Drop(IDropInfo dropInfo)
        {
            NoteViewModel sourceItem = dropInfo.Data as NoteViewModel;
            NoteViewModel targetItem = dropInfo.TargetItem as NoteViewModel;

            sourceItem.Group = dropInfo.TargetGroup.Name.ToString();

            // Changing group data at runtime isn't handled well: force a refresh on the collection view.
            if (dropInfo.TargetCollection is IEditableCollectionView)
            {
                ((IEditableCollectionView)dropInfo.TargetCollection).EditItem(sourceItem);
                ((IEditableCollectionView)dropInfo.TargetCollection).CommitEdit();
            }

            // Update databases
            AllNotes.LocalStorage.UpdateNote(sourceItem.NoteData);

            // Select the moved note
            AllNotes.SelectedNote = sourceItem;
        }
    }
}

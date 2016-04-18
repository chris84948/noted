using JustMVVM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace NotedUI.UI.ViewModels
{
    public class AllNotesViewModel : MVVMBase
    {
        private ObservableCollection<NoteViewModel> _notes;

        private ICollectionView _view;
        public ICollectionView View
        {
            get { return _view; }
        }

        public AllNotesViewModel()
        {
            _notes = new ObservableCollection<NoteViewModel>()
            {
                new NoteViewModel("1,", DateTime.Now.Subtract(TimeSpan.FromDays(1)), "Note 1", "Group 1"),
                new NoteViewModel("1,", DateTime.Now.Subtract(TimeSpan.FromMinutes(34)), "Note 2", "Group 1"),
                new NoteViewModel("1,", DateTime.Now, "Note 3", "Group 2"),
                new NoteViewModel("1,", DateTime.Now.Subtract(TimeSpan.FromDays(14)), "Note 4", "Group 2"),
                new NoteViewModel("1,", DateTime.Now.Subtract(TimeSpan.FromHours(7)), "Note 5", "Group 2"),
                new NoteViewModel("1,", DateTime.Now.Subtract(TimeSpan.FromSeconds(35)), "Note 6", "Group 2")
            };

            _view = CollectionViewSource.GetDefaultView(_notes);
            _view.SortDescriptions.Add(new SortDescription("Title", ListSortDirection.Ascending));
            _view.GroupDescriptions.Add(new PropertyGroupDescription("Folder"));

            //PropertyGroupDescription groupDescription = new PropertyGroupDescription("Group");
            //groupDescription.GroupNames.Add("GROUP 1");
            //groupDescription.GroupNames.Add("GROUP 2");
            //groupDescription.GroupNames.Add("GROUP 3");
            //view.GroupDescriptions.Add(groupDescription);
        }
    }

    public class NoteSorter : IComparer
    {
        public int Compare(object a, object b)
        {
            NoteViewModel noteA = a as NoteViewModel;
            NoteViewModel noteB = b as NoteViewModel;

            return noteA.Title.CompareTo(noteB.Title);
        }
    }
}

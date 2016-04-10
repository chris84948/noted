using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace NotedUI.UI.Screens
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : UserControl
    {
        public Home()
        {
            InitializeComponent();
        }

        private void storyboardShowSearch_Completed(object sender, EventArgs e)
        {
            SearchBox.Focus();
            SearchBox.SelectAll();
        }

        private void me_Loaded(object sender, RoutedEventArgs e)
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lvNotes.ItemsSource);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("Group");
            groupDescription.GroupNames.Add("GROUP 1");
            groupDescription.GroupNames.Add("GROUP 2");
            groupDescription.GroupNames.Add("GROUP 3");
            view.GroupDescriptions.Add(groupDescription);
        }
    }

    public class Note : JustMVVM.MVVMBase
    {
        public string Group { get; set; }
        public string Title { get; set; }
        public DateTime LastModified { get; set; }

        private bool _isMarkedForRemoval;
        public bool IsMarkedForRemoval
        {
            get { return _isMarkedForRemoval; }
            set
            {
                _isMarkedForRemoval = value;
                OnPropertyChanged();
            }
        }
    }

    public class ListData
    {
        public ObservableCollection<Note> AllNotes { get; set; }
        public Note SelectedNote { get; set; }

        public ListData(ObservableCollection<Note> allNotes, Note selectedNote)
        {
            AllNotes = allNotes;
            SelectedNote = selectedNote;
        }
    }
}

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
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("Folder");
            //groupDescription.GroupNames.Add("GROUP 3");
            view.GroupDescriptions.Add(groupDescription);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

    public class Group
    {
        public string Name { get; set; }
        public List<Note> Notes { get; set; }
    }

    public class Note
    {
        public string Group { get; set; }
        public string Title { get; set; }
        public DateTime LastModified { get; set; }
    }
}

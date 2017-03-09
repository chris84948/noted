using NotedUI.Models;
using NotedUI.UI.ViewModels;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace NotedUI.UI.Components
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : UserControl
    {
        public MainMenu()
        {
            InitializeComponent();
        }

        private void buttonExport_Click(object sender, RoutedEventArgs e)
        {
            popupExport.IsOpen = false;
        }

        private async void buttonAdd_Checked(object sender, RoutedEventArgs e)
        {
            var homeVM = DataContext as HomeViewModel;

            if (homeVM == null)
                return;

            if (homeVM.AllNotes.AllGroups.Groups.Count == 0)
            {
                buttonAdd.IsChecked = false;
                await homeVM.MainCommands.AddGroupToDatabase("NOTES");
                homeVM.MainCommands.AddNoteCommand.Execute("NOTES");
            }
        }
    }
}
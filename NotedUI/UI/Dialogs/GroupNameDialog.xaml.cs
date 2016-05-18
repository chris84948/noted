using System.Windows;
using System.Windows.Controls;

namespace NotedUI.UI.Dialogs
{
    /// <summary>
    /// Interaction logic for LinkDialog.xaml
    /// </summary>
    public partial class GroupNameDialog : UserControl
    {
        public GroupNameDialog()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            tbGroupName.Focus();
            tbGroupName.SelectAll();
        }
    }
}

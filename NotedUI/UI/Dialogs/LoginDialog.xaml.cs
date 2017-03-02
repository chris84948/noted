using NotedUI.Controls;
using System.Windows;
using System.Windows.Controls;

namespace NotedUI.UI.Dialogs
{
    /// <summary>
    /// Interaction logic for LinkDialog.xaml
    /// </summary>
    public partial class LoginDialog : NotedWindow
    {
        public LoginDialog()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            tbUsername.Focus();
            tbUsername.SelectAll();
        }
    }
}

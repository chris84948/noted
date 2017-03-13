using NotedUI.Controls;
using NotedUI.UI.DialogViewModels;
using NotedUI.UI.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace NotedUI.UI.Dialogs
{
    /// <summary>
    /// Interaction logic for LinkDialog.xaml
    /// </summary>
    public partial class LoginDialog : NotedWindow
    {
        public LoginDialog(string errorMessage)
        {
            InitializeComponent();

            (DataContext as LoginDialogViewModel).ErrorMessage = errorMessage;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            tbUsername.Focus();
            tbUsername.SelectAll();
        }
    }
}

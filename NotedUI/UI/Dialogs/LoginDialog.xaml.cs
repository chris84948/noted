using NotedUI.Controls;
using NotedUI.UI.DialogViewModels;
using NotedUI.UI.ViewModels;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace NotedUI.UI.Dialogs
{
    /// <summary>
    /// Interaction logic for LinkDialog.xaml
    /// </summary>
    public partial class LoginDialog : NotedWindow
    {
        private bool _initialStartup;

        public LoginDialog(string errorMessage, bool initialStartup)
        {
            InitializeComponent();
            _initialStartup = initialStartup;

            (DataContext as LoginDialogViewModel).ErrorMessage = errorMessage;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            tbUsername.Focus();
            tbUsername.SelectAll();

            if (!_initialStartup)
                lblStartup.Visibility = Visibility.Collapsed;
        }

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }
}

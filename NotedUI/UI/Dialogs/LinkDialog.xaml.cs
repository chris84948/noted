using System.Windows;
using System.Windows.Controls;

namespace NotedUI.UI.Dialogs
{
    /// <summary>
    /// Interaction logic for LinkDialog.xaml
    /// </summary>
    public partial class LinkDialog : UserControl
    {
        public LinkDialog()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (tbDescription.Text.Length > 0)
            {
                tbAddress.Focus();
                tbAddress.Select(tbAddress.Text.Length, 0);
            }
            else
            {
                tbDescription.Focus();
                tbDescription.SelectAll();
            }
        }
    }
}

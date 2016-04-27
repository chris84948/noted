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

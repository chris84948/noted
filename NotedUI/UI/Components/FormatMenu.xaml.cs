using ICSharpCode.AvalonEdit;
using JustMVVM;
using NotedUI.Models;
using NotedUI.UI.ViewModels;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace NotedUI.UI.Components
{
    /// <summary>
    /// Interaction logic for FormatMenu.xaml
    /// </summary>
    public partial class FormatMenu : UserControl
    {
        public FormatMenu()
        {
            InitializeComponent();
        }

        private void buttonFind_Checked(object sender, RoutedEventArgs e)
        {
            if (buttonReplace.IsChecked == true)
                buttonReplace.IsChecked = false;

            (DataContext as HomeViewModel)?.Formatting?.FindDialogCommand?.Execute(true);
        }

        private void buttonFind_Unchecked(object sender, RoutedEventArgs e)
        {
            (DataContext as HomeViewModel)?.Formatting?.FindDialogCommand?.Execute(false);
        }

        private void buttonReplace_Checked(object sender, RoutedEventArgs e)
        {
            if (buttonFind.IsChecked == true)
                buttonFind.IsChecked = false;

            (DataContext as HomeViewModel)?.Formatting?.ReplaceDialogCommand?.Execute(true);
        }

        private void buttonReplace_Unchecked(object sender, RoutedEventArgs e)
        {
            (DataContext as HomeViewModel)?.Formatting?.ReplaceDialogCommand?.Execute(false);
        }
    }
}

using ICSharpCode.AvalonEdit;
using NotedUI.DataStorage;
using NotedUI.UI.Dialogs;
using NotedUI.UI.ViewModels;
using NotedUI.Utilities;
using System;
using System.Windows;

namespace NotedUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static ICloudStorage Cloud { get; set; }
        public static ILocalStorage Local { get; set; }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // This frees up the Ctrl-I input binding for italics
            AvalonEditCommands.IndentSelection.InputGestures.Clear();

            Local = new SQLiteStorage();
            Local.Initialize();
            Cloud = new GoogleDriveStorage();

            string username = Local.GetUsername().Result;

            if (String.IsNullOrWhiteSpace(username))
            {
                ShowLoginDialog();
            }
            else
            {
                Cloud.Connect(username);
                ShowMainWindow();
            }
        }

        private void ShowLoginDialog()
        {
            LoginDialog dialog = new LoginDialog();
            dialog.Show();
            dialog.Activate();

            (dialog.DataContext as LoginDialogViewModel).DialogClosed += (d) => 
            {
                App.Current.Dispatcher.Invoke(() =>
                {
                    dialog.Close();
                    ShowMainWindow();
                });
            };
        }

        private void ShowMainWindow()
        {
            MainWindow window = new MainWindow();
            window.Closed += (s, e) => App.Current.Shutdown();
            window.Show();
            window.Activate();
        }
    }
}

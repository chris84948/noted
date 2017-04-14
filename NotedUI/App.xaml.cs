using ICSharpCode.AvalonEdit;
using NotedUI.DataStorage;
using NotedUI.UI.Dialogs;
using NotedUI.UI.DialogViewModels;
using NotedUI.UI.ViewModels;
using NotedUI.Utilities;
using Squirrel;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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

            string username = Local.GetUsername().Result;
            Cloud = new GoogleDriveStorage(username);

            if (String.IsNullOrWhiteSpace(username))
            {
                ShowLoginDialog();
            }
            else
            {
                Task.Run(async () =>
                {
                    if (!Cloud.IsInternetConnected() || await Cloud.Connect())
                        App.Current.Dispatcher.Invoke(() => ShowMainWindow());
                    else
                        App.Current.Dispatcher.Invoke(() => ShowLoginDialog("Timeout attempting to login. Please try again."));
                });
            }

            if (!Debugger.IsAttached)
                Task.Run(() => CheckForUpdates());
        }

        private static void ShowLoginDialog(string errorMessage = null)
        {
            LoginDialog dialog = new LoginDialog(errorMessage);
            dialog.Show();
            dialog.Activate();

            (dialog.DataContext as LoginDialogViewModel).DialogClosed += (d) => 
            {
                App.Current.Dispatcher.Invoke(() => ShowMainWindow(dialog));
            };
        }

        private static void ShowMainWindow(Window currentWindow = null)
        {
            MainWindow window = new MainWindow();
            window.Show();
            window.Activate();

            currentWindow?.Close();
        }

        public static void RestartApplication()
        {
            var mainWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);

            ShowLoginDialog();

            mainWindow?.Close();
        }

        private async void CheckForUpdates()
        {
            if (Directory.Exists(@"C:\Github\NotedUI\Nuget_Builds\Releases"))
            {
                using (var mgr = new UpdateManager(@"C:\Github\NotedUI\Nuget_Builds\Releases"))
                    await mgr.UpdateApp();
            }
        }
    }
}
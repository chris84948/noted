using ICSharpCode.AvalonEdit;
using NotedUI.DataStorage;
using NotedUI.UI.Dialogs;
using NotedUI.UI.DialogViewModels;
using NotedUI.UI.ViewModels;
using NotedUI.Utilities;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
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
        public static string AppPath { get; private set; }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Task.Run(function: Application_StartupAsync);
        }

        private async Task Application_StartupAsync()
        {
            // This frees up the Ctrl-I input binding for italics
            AvalonEditCommands.IndentSelection.InputGestures.Clear();

            AppPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            Local = new SQLiteStorage();
            await Local.Initialize();

            string username = Local.GetUsername().Result;
            Cloud = new GoogleDriveStorage(username);

            if (String.IsNullOrWhiteSpace(username))
            {
                App.Current.Dispatcher.Invoke(() => ShowLoginDialog(true));
            }
            else
            {
                if (!Cloud.IsInternetConnected() || await Cloud.Connect())
                    App.Current.Dispatcher.Invoke(() => ShowMainWindow());
                else
                    App.Current.Dispatcher.Invoke(() => ShowLoginDialog(false, "Timeout attempting to login. Please try again."));
            }
        }

        private static void ShowLoginDialog(bool initialStartup, string errorMessage = null)
        {
            LoginDialog dialog = new LoginDialog(errorMessage, initialStartup);
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

            ShowLoginDialog(false);

            mainWindow?.Close();
        }
    }
}
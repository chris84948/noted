using ICSharpCode.AvalonEdit;
using NotedUI.DataStorage;
using NotedUI.UI.Dialogs;
using NotedUI.UI.DialogViewModels;
using NotedUI.UI.ViewModels;
using NotedUI.Utilities;
using System;
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
            Cloud = new GoogleDriveStorage();

            string username = Local.GetUsername().Result;

            if (String.IsNullOrWhiteSpace(username))
            {
                ShowLoginDialog();
            }
            else
            {
                Task.Run(async () =>
                {
                    if (await Cloud.Connect(username))
                        App.Current.Dispatcher.Invoke(() => ShowMainWindow());
                    else
                        App.Current.Dispatcher.Invoke(() => ShowLoginDialog("Timeout attempting to login. Please try again."));
                });
            }
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
    }
}

using ICSharpCode.AvalonEdit;
using NotedUI.Utilities;
using System.Windows;

namespace NotedUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // This frees up the Ctrl-I input binding for italics
            AvalonEditCommands.IndentSelection.InputGestures.Clear();
        }
    }
}

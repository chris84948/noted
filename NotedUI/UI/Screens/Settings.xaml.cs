using System.Windows;
using System.Windows.Controls;

namespace NotedUI.UI.Screens
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : UserControl
    {
        public static readonly RoutedEvent CloseClickedEvent =
            EventManager.RegisterRoutedEvent("CloseClicked",
                                             RoutingStrategy.Bubble,
                                             typeof(RoutedEventHandler),
                                             typeof(Settings));

        public event RoutedEventHandler CloseClicked
        {
            add { AddHandler(CloseClickedEvent, value); }
            remove { RemoveHandler(CloseClickedEvent, value); }
        }
        
        public Settings()
        {
            InitializeComponent();
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            RoutedEventArgs args = new RoutedEventArgs(CloseClickedEvent);
            RaiseEvent(args);
        }
    }
}

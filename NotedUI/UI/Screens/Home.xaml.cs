using mshtml;
using NotedUI.Resources.AvalonHighlighting;
using NotedUI.UI.Storyboards;
using NotedUI.UI.ViewModels;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace NotedUI.UI.Screens
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : UserControl
    {
        public static readonly RoutedEvent EnterPressedEvent =
            EventManager.RegisterRoutedEvent("EnterPressed",
                                             RoutingStrategy.Bubble,
                                             typeof(RoutedEventHandler),
                                             typeof(Home));

        public event RoutedEventHandler EnterPressed
        {
            add { AddHandler(EnterPressedEvent, value); }
            remove { RemoveHandler(EnterPressedEvent, value); }
        }

        public static readonly RoutedEvent FindDialogShownEvent =
            EventManager.RegisterRoutedEvent("FindDialogShown",
                                             RoutingStrategy.Bubble,
                                             typeof(RoutedEventHandler),
                                             typeof(Home));

        public event RoutedEventHandler FindDialogShown
        {
            add { AddHandler(FindDialogShownEvent, value); }
            remove { RemoveHandler(FindDialogShownEvent, value); }
        }

        private double _markdownScrollPercent;

        public Home()
        {
            InitializeComponent();
        }

        private void storyboardShowSearch_Completed(object sender, EventArgs e)
        {
            SearchBox.Focus();
            SearchBox.SelectAll();
        }

        private void storyboardShowFindDialog_Completed(object sender, EventArgs e)
        {
            // Raise an event to call a command to focus the Find/Repalce dialog
            RoutedEventArgs routedArgs = new RoutedEventArgs(FindDialogShownEvent);
            me.RaiseEvent(routedArgs);
        }

        private void me_Loaded(object sender, RoutedEventArgs e)
        {
            var homeVM = DataContext as HomeViewModel;

            // This event forces the markdown preview to be the same size as the editor
            gridNote.SizeChanged += (s, args) =>
            {
                tbMarkdownPanel.Width = args.NewSize.Width / 2;

                if (tbMarkdownPanel.Margin.Right < 0)
                    tbMarkdownPanel.Margin = new Thickness(0, 0, -args.NewSize.Width / 2, 0);
            };

            // This event passes the Enter pressed key to handle continuing lists
            tbNote.PreviewKeyDown += (s, args) =>
            {
                if (args.Key == System.Windows.Input.Key.Enter)
                    tbNote.TextChanged += tbNote_EnterHandler;
            };

            // HACK Might want to change this sometime later
            homeVM.AllNotes.TextEditor = tbNote;
        }

        private void tbNote_EnterHandler(object sender, EventArgs args)
        {
            tbNote.TextChanged -= tbNote_EnterHandler;

            RoutedEventArgs routedArgs = new RoutedEventArgs(EnterPressedEvent);
            me.RaiseEvent(routedArgs);
        }

        private void MainMenu_ShowPreviewChanged(object sender, RoutedEventArgs e)
        {
            Storyboard sb = null;

            if (MainMenu.ShowPreview)
                sb = MarkdownPreviewStoryboards.GetShow(tbMarkdownPanel, gridNote, gridSplitterMarkdown);
            else
                sb = MarkdownPreviewStoryboards.GetHide(tbMarkdownPanel, gridNote, gridSplitterMarkdown);

            sb.Begin();
        }
        
        private void tbNote_Loaded(object sender, RoutedEventArgs e)
        {
            tbNote.SyntaxHighlighting = ResourceLoader.LoadHighlightingDefinition("Markdown");

            tbNote.Focus();
            tbNote.Select(0, 0);
        }

        private void tbMarkdown_Navigating(object sender, System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            if (e.Uri == null)
                return;

            // cancel loading in this browser control
            e.Cancel = true;

            Process.Start(new ProcessStartInfo(e.Uri.ToString()));
        }

        private void lvNotes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Task.Run(() =>
            {
                Task.Delay(20);
                App.Current.Dispatcher.Invoke(() =>
                {
                    tbNote.Focus();
                    tbNote.Select(tbNote.Text.Length, 0);
                });
            });
        }

        private void tbNote_ScrollPositionChanged(object sender, ScrollChangedEventArgs e)
        {
            _markdownScrollPercent = PercentScroll(e);
            SetScrollOffset(_markdownScrollPercent);
        }

        public void SetScrollOffset(double scrollPercent)
        {
            var document2 = (IHTMLDocument2)tbMarkdown.Document;
            var document3 = (IHTMLDocument3)tbMarkdown.Document;

            if (document3?.documentElement != null)
            {
                if (scrollPercent > 0.99)
                    scrollPercent = 1.1; // deal with round off at end of scroll

                var body = document2.body;
                var scrollHeight = ((IHTMLElement2)body).scrollHeight - document3.documentElement.offsetHeight;

                int scrollPosition = (int)Math.Ceiling(scrollPercent * scrollHeight);

                document2.parentWindow.scroll(0, scrollPosition);
            }
        }

        private static double PercentScroll(ScrollChangedEventArgs e)
        {
            var y = e.ExtentHeight - e.ViewportHeight;
            return e.VerticalOffset / ((Math.Abs(y) < .000001) ? 1 : y);
        }
    }
}
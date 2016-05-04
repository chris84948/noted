using mshtml;
using NotedUI.AttachedBehaviors;
using NotedUI.Resources.AvalonHighlighting;
using NotedUI.UI.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
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

            if (homeVM?.ShowPreviewOnLoad == true)
            {
                homeVM.ShowPreviewOnLoad = false;
                MainMenu.ShowPreview = true;
            }

            // This event forces the markdown preview to be the same size as the editor
            gridNote.SizeChanged += (s, args) =>
            {
                tbMarkdown.Width = args.NewSize.Width / 2;

                if (tbMarkdown.Margin.Right < 0)
                    tbMarkdown.Margin = new Thickness(0, 0, -args.NewSize.Width / 2, 0);
            };

            // This event passes the Enter pressed key to handle continuing lists
            tbNote.PreviewKeyDown += (s, args) =>
            {
                if (args.Key == System.Windows.Input.Key.Enter)
                    tbNote.TextChanged += tbNote_TextChangedEvent;
            };

            tbNote.Text = @"## Welcome to MarkdownPad 2 ##

**MarkdownPad** is a full-featured Markdown editor for Windows.

### Built exclusively for Markdown ###

Enjoy first-class Markdown support with easy access to  Markdown syntax and convenient keyboard shortcuts.

Give them a try:

- **Bold** (`Ctrl+B`) and *Italic* (`Ctrl+I`)
- Quotes (`Ctrl+Q`)
- Code blocks (`Ctrl+K`)
- Headings 1, 2, 3 (`Ctrl+1`, `Ctrl+2`, `Ctrl+3`)
- Lists (`Ctrl+U` and `Ctrl+Shift+O`)

### See your changes instantly with LivePreview ###

Don't guess if your [hyperlink syntax](http://markdownpad.com) is correct; LivePreview will show you exactly what your document looks like every time you press a key.

### Make it your own ###

Fonts, color schemes, layouts and stylesheets are all 100% customizable so you can turn MarkdownPad into your perfect editor.

### A robust editor for advanced Markdown users ###

MarkdownPad supports multiple Markdown processing engines, including standard Markdown, Markdown Extra (with Table support) and GitHub Flavored Markdown.

With a tabbed document interface, PDF export, a built-in image uploader, session management, spell check, auto-save, syntax highlighting and a built-in CSS management interface, there's no limit to what you can do with MarkdownPad.";

            tbNote.Focus();
            tbNote.Select(tbNote.Text.Length, 0);
        }

        private void tbNote_TextChangedEvent(object sender, EventArgs args)
        {
            tbNote.TextChanged -= tbNote_TextChangedEvent;

            RoutedEventArgs routedArgs = new RoutedEventArgs(EnterPressedEvent);
            me.RaiseEvent(routedArgs);
        }

        private void MainMenu_ShowPreviewChanged(object sender, RoutedEventArgs e)
        {
            Storyboard sb = null;

            if (MainMenu.ShowPreview)
                sb = GetShowPreviewStoryboard();

            else
                sb = GetHidePreviewStoryboard();

            sb.Begin();
        }
        
        private Storyboard GetShowPreviewStoryboard()
        {
            // Fixes the first time width to half the screen
            tbMarkdown.Width = gridNote.ActualWidth / 2;
            tbMarkdown.Margin = new Thickness(0, 0, -tbMarkdown.Width, 0);

            var visAnim = new ObjectAnimationUsingKeyFrames()
            {
                Duration = new Duration(TimeSpan.FromMilliseconds(500)),
                KeyFrames = new ObjectKeyFrameCollection()
                {
                    new DiscreteObjectKeyFrame(Visibility.Visible, KeyTime.FromPercent(1))
                }
            };
            Storyboard.SetTarget(visAnim, gridSplitterMarkdown);
            Storyboard.SetTargetProperty(visAnim, new PropertyPath("(FrameworkElement.Visibility)"));

            var marginAnim = new ThicknessAnimationUsingKeyFrames()
            {
                Duration = new Duration(TimeSpan.FromMilliseconds(500)),
                KeyFrames = new ThicknessKeyFrameCollection()
                {
                    new EasingThicknessKeyFrame(new Thickness(0), KeyTime.FromPercent(1))
                        { EasingFunction = new SineEase() { EasingMode = EasingMode.EaseOut } }
                }
            };
            Storyboard.SetTarget(marginAnim, tbMarkdown);
            Storyboard.SetTargetProperty(marginAnim, new PropertyPath("(FrameworkElement.Margin)"));

            var widthAnim = new DoubleAnimationUsingKeyFrames()
            {
                Duration = new Duration(TimeSpan.FromMilliseconds(500)),
                KeyFrames = new DoubleKeyFrameCollection()
                {
                    new SplineDoubleKeyFrame(tbMarkdown.ActualWidth, KeyTime.FromPercent(0.999)),
                    new SplineDoubleKeyFrame(Double.NaN, KeyTime.FromPercent(1))
                }
            };
            Storyboard.SetTarget(marginAnim, tbMarkdown);
            Storyboard.SetTargetProperty(marginAnim, new PropertyPath("(FrameworkElement.Margin)"));

            var sb = new Storyboard()
            {
                Children = new TimelineCollection() { visAnim, marginAnim }
            };

            sb.Completed += (sender, args) =>
            {
                tbMarkdown.Width = gridNote.ActualWidth / 2;
                tbMarkdown.Margin = new Thickness(0, 0, 0, 0);
            };

            return sb;
        }

        private Storyboard GetHidePreviewStoryboard()
        {
            tbMarkdown.Width = tbMarkdown.ActualWidth;

            var visAnim = new ObjectAnimationUsingKeyFrames()
            {
                KeyFrames = new ObjectKeyFrameCollection()
                {
                    new DiscreteObjectKeyFrame(Visibility.Collapsed, KeyTime.FromPercent(0))
                }
            };
            Storyboard.SetTarget(visAnim, gridSplitterMarkdown);
            Storyboard.SetTargetProperty(visAnim, new PropertyPath("(FrameworkElement.Visibility)"));

            var marginAnim = new ThicknessAnimationUsingKeyFrames()
            {
                Duration = new Duration(TimeSpan.FromMilliseconds(500)),
                KeyFrames = new ThicknessKeyFrameCollection()
                {
                    new EasingThicknessKeyFrame(new Thickness(0, 0, -tbMarkdown.ActualWidth, 0), KeyTime.FromPercent(1))
                        { EasingFunction = new SineEase() { EasingMode = EasingMode.EaseOut } }
                }
            };
            Storyboard.SetTarget(marginAnim, tbMarkdown);
            Storyboard.SetTargetProperty(marginAnim, new PropertyPath("(FrameworkElement.Margin)"));

            var sb = new Storyboard()
            {
                Children = new TimelineCollection() { visAnim, marginAnim }
            };

            return sb;
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


        private void tbNote_ScrollPositionChanged(object sender, ScrollChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Vertical Position Changed - " + e.VerticalChange);

            SetScrollOffset(e);
        }

        public void SetScrollOffset(ScrollChangedEventArgs ea)
        {
            var document2 = (IHTMLDocument2)tbMarkdown.Document;
            var document3 = (IHTMLDocument3)tbMarkdown.Document;

            if (document3?.documentElement != null)
            {
                var percentToScroll = PercentScroll(ea);
                if (percentToScroll > 0.99) percentToScroll = 1.1; // deal with round off at end of scroll
                var body = document2.body;
                var scrollHeight = ((IHTMLElement2)body).scrollHeight - document3.documentElement.offsetHeight;
                document2.parentWindow.scroll(0, (int)Math.Ceiling(percentToScroll * scrollHeight));
            }
        }

        private static double PercentScroll(ScrollChangedEventArgs e)
        {
            var y = e.ExtentHeight - e.ViewportHeight;
            return e.VerticalOffset / ((Math.Abs(y) < .000001) ? 1 : y);
        }
    }
}

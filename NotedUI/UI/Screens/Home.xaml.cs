using System;
using System.Collections.ObjectModel;
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
        private bool changedWidthOfMarkdown = false;

        public Home()
        {
            InitializeComponent();
        }

        private void storyboardShowSearch_Completed(object sender, EventArgs e)
        {
            SearchBox.Focus();
            SearchBox.SelectAll();
        }

        private void me_Loaded(object sender, RoutedEventArgs e)
        {
            tbMarkdown.Width = gridNote.ActualWidth / 2;
            tbMarkdown.Margin = new Thickness(0, 0, -tbMarkdown.Width, 0);
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
        
        private void GridSplitterMarkdown_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            // Only run this event once to fix the width of the markdown field
            gridSplitterMarkdown.DragDelta -= GridSplitterMarkdown_DragDelta;
            tbMarkdown.Width = Double.NaN;
            changedWidthOfMarkdown = true;
        }

        private Storyboard GetShowPreviewStoryboard()
        {
            // Really hacky hack to make the dragging work
            gridSplitterMarkdown.DragDelta += GridSplitterMarkdown_DragDelta;

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
                    new EasingThicknessKeyFrame(new Thickness(0, 0, (changedWidthOfMarkdown ? -2 : -1) * tbMarkdown.ActualWidth, 0), KeyTime.FromPercent(1))
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


        private void MainMenu_SettingsClicked(object sender, RoutedEventArgs e)
        {
            var marginAnim = new ThicknessAnimation(new Thickness(0), new Duration(TimeSpan.FromMilliseconds(500)));
            marginAnim.EasingFunction = new SineEase() { EasingMode = EasingMode.EaseOut };
            SettingsScreen.BeginAnimation(MarginProperty, marginAnim);
        }

        private void Settings_CloseClicked(object sender, RoutedEventArgs e)
        {
            var marginAnim = new ThicknessAnimation(new Thickness(-SettingsScreen.ActualWidth * 2, 0, 0, 0), new Duration(TimeSpan.FromMilliseconds(500)));
            marginAnim.EasingFunction = new SineEase() { EasingMode = EasingMode.EaseOut };
            SettingsScreen.BeginAnimation(MarginProperty, marginAnim);
        }
    }
}

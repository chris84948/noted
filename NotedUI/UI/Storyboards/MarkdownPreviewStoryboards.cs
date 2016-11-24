using AirspaceFixer;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace NotedUI.UI.Storyboards
{
    public class MarkdownPreviewStoryboards
    {
        public static Storyboard GetShow(AirspacePanel tbMarkdownPanel, Grid gridNote, Rectangle gridSplitterMarkdown)
        {
            // Fixes the first time width to half the screen
            tbMarkdownPanel.Width = gridNote.ActualWidth / 2;
            tbMarkdownPanel.Margin = new Thickness(0, 0, -tbMarkdownPanel.Width, 0);

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
            Storyboard.SetTarget(marginAnim, tbMarkdownPanel);
            Storyboard.SetTargetProperty(marginAnim, new PropertyPath("(FrameworkElement.Margin)"));

            var widthAnim = new DoubleAnimationUsingKeyFrames()
            {
                Duration = new Duration(TimeSpan.FromMilliseconds(500)),
                KeyFrames = new DoubleKeyFrameCollection()
                {
                    new SplineDoubleKeyFrame(tbMarkdownPanel.ActualWidth, KeyTime.FromPercent(0.999)),
                    new SplineDoubleKeyFrame(Double.NaN, KeyTime.FromPercent(1))
                }
            };
            Storyboard.SetTarget(marginAnim, tbMarkdownPanel);
            Storyboard.SetTargetProperty(marginAnim, new PropertyPath("(FrameworkElement.Margin)"));

            var sb = new Storyboard()
            {
                Children = new TimelineCollection() { visAnim, marginAnim }
            };

            sb.Completed += (sender, args) =>
            {
                tbMarkdownPanel.Width = gridNote.ActualWidth / 2;
                tbMarkdownPanel.Margin = new Thickness(0, 0, 0, 0);
            };

            return sb;
        }

        public static Storyboard GetHide(AirspacePanel tbMarkdownPanel, Grid gridNote, Rectangle gridSplitterMarkdown)
        {
            tbMarkdownPanel.Width = tbMarkdownPanel.ActualWidth;

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
                    new EasingThicknessKeyFrame(new Thickness(0, 0, -tbMarkdownPanel.ActualWidth, 0), KeyTime.FromPercent(1))
                        { EasingFunction = new SineEase() { EasingMode = EasingMode.EaseOut } }
                }
            };
            Storyboard.SetTarget(marginAnim, tbMarkdownPanel);
            Storyboard.SetTargetProperty(marginAnim, new PropertyPath("(FrameworkElement.Margin)"));

            var sb = new Storyboard()
            {
                Children = new TimelineCollection() { visAnim, marginAnim }
            };

            return sb;
        }
    }
}

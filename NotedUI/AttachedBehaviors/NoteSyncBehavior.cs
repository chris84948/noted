using NotedUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace NotedUI.AttachedBehaviors
{
    public class NoteSyncBehavior
    {
        public static readonly DependencyProperty SyncStateProperty =
            DependencyProperty.RegisterAttached("SyncState",
                                                typeof(eNoteState),
                                                typeof(NoteSyncBehavior),
                                                new UIPropertyMetadata(eNoteState.Normal, SyncStateChanged));

        public static eNoteState GetSyncState(DependencyObject obj)
        {
            return (eNoteState)obj.GetValue(SyncStateProperty);
        }

        public static void SetSyncState(DependencyObject obj, eNoteState value)
        {
            obj.SetValue(SyncStateProperty, value);
        }

        private static void SyncStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ListViewItem item = d as ListViewItem;
            eNoteState oldState = (eNoteState)e.OldValue;
            eNoteState newState = (eNoteState)e.NewValue;

            if (oldState == eNoteState.Normal && newState == eNoteState.Changed)
            {
                var sb = GetOutOfSyncStoryboard();

                sb.Begin(item, item.Template);
            }
            else if (oldState == eNoteState.Changed && newState == eNoteState.Syncing)
            {
                var sb = GetStartSyncStoryboard();

                sb.Completed += (_, __) =>
                {
                    RunSyncingStoryboard(d, item);
                };

                sb.Begin(item, item.Template);
            }
        }

        private static void RunSyncingStoryboard(DependencyObject d, ListViewItem item)
        {
            var sb = GetSyncingStoryboard();

            sb.Completed += (_, __) =>
            {
                var state = GetSyncState(d);

                if (state == eNoteState.Syncing)
                    RunSyncingStoryboard(d, item);

                else if (state == eNoteState.SyncComplete)
                    RunSyncCompleteStoryboard(d, item);
            };

            sb.Begin(item, item.Template);
        }

        private static void RunSyncCompleteStoryboard(DependencyObject d, ListViewItem item)
        {
            var sb = GetSyncCompleteStoryboard();

            sb.Completed += (_, __) =>
            {
                SetSyncState(d, eNoteState.Normal);
            };

            sb.Begin(item, item.Template);
        }

        public static Storyboard GetOutOfSyncStoryboard()
        {
            var visAnim = new ObjectAnimationUsingKeyFrames()
            {
                KeyFrames = new ObjectKeyFrameCollection()
                {
                    new DiscreteObjectKeyFrame(Visibility.Visible, KeyTime.FromPercent(0))
                }
            };
            Storyboard.SetTargetName(visAnim, "IconSync");
            Storyboard.SetTargetProperty(visAnim, new PropertyPath("(UIElement.Visibility)"));

            var scaleXAnim = new DoubleAnimationUsingKeyFrames()
            {
                Duration = new Duration(TimeSpan.FromMilliseconds(400)),
                KeyFrames = new DoubleKeyFrameCollection()
                {
                    new SplineDoubleKeyFrame(0, KeyTime.FromPercent(0)),
                    new EasingDoubleKeyFrame(1, KeyTime.FromPercent(1)) { EasingFunction = new BackEase() { Amplitude = 0.8, EasingMode = EasingMode.EaseOut } }
                }
            };
            Storyboard.SetTargetName(scaleXAnim, "IconSync");
            Storyboard.SetTargetProperty(scaleXAnim, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleX)"));

            var scaleYAnim = new DoubleAnimationUsingKeyFrames()
            {
                Duration = new Duration(TimeSpan.FromMilliseconds(400)),
                KeyFrames = new DoubleKeyFrameCollection()
                {
                    new SplineDoubleKeyFrame(0, KeyTime.FromPercent(0)),
                    new EasingDoubleKeyFrame(1, KeyTime.FromPercent(1)) { EasingFunction = new BackEase() { Amplitude = 0.8, EasingMode = EasingMode.EaseOut } }
                }
            };
            Storyboard.SetTargetName(scaleYAnim, "IconSync");
            Storyboard.SetTargetProperty(scaleYAnim, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleY)"));

            return new Storyboard()
            {
                Children = new TimelineCollection() { visAnim, scaleXAnim, scaleYAnim }
            };
        }

        public static Storyboard GetStartSyncStoryboard()
        {
            var rotateAnim = new DoubleAnimationUsingKeyFrames()
            {
                Duration = new Duration(TimeSpan.FromMilliseconds(2000)),
                KeyFrames = new DoubleKeyFrameCollection()
                {
                    new SplineDoubleKeyFrame(0, KeyTime.FromPercent(0)),
                    new SplineDoubleKeyFrame(0, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(400))),
                    new EasingDoubleKeyFrame(-360, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(1400))) { EasingFunction = new PowerEase() { EasingMode = EasingMode.EaseIn } },
                    new SplineDoubleKeyFrame(-720, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(2000))),
                }
            };
            Storyboard.SetTargetName(rotateAnim, "IconSync");
            Storyboard.SetTargetProperty(rotateAnim, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[1].(RotateTransform.Angle)"));

            return new Storyboard()
            {
                Children = new TimelineCollection() { rotateAnim }
            };
        }

        public static Storyboard GetSyncingStoryboard()
        {
            var rotateAnim = new DoubleAnimationUsingKeyFrames()
            {
                Duration = new Duration(TimeSpan.FromMilliseconds(300)),
                KeyFrames = new DoubleKeyFrameCollection()
                {
                    new SplineDoubleKeyFrame(0, KeyTime.FromPercent(0)),
                    new SplineDoubleKeyFrame(-180, KeyTime.FromPercent(1))
                }
            };
            Storyboard.SetTargetName(rotateAnim, "IconSync");
            Storyboard.SetTargetProperty(rotateAnim, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[1].(RotateTransform.Angle)"));

            return new Storyboard()
            {
                Children = new TimelineCollection() { rotateAnim }
            };
        }

        public static Storyboard GetSyncCompleteStoryboard()
        {
            var scaleXAnim = new DoubleAnimationUsingKeyFrames()
            {
                Duration = new Duration(TimeSpan.FromMilliseconds(201)),
                KeyFrames = new DoubleKeyFrameCollection()
                {
                    new SplineDoubleKeyFrame(1, KeyTime.FromPercent(0)),
                    new EasingDoubleKeyFrame(0, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(200))) { EasingFunction = new QuarticEase() { EasingMode = EasingMode.EaseIn } },
                    new SplineDoubleKeyFrame(1, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(201)))
                }
            };
            Storyboard.SetTargetName(scaleXAnim, "IconSync");
            Storyboard.SetTargetProperty(scaleXAnim, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleX)"));


            var scaleYAnim = new DoubleAnimationUsingKeyFrames()
            {
                Duration = new Duration(TimeSpan.FromMilliseconds(201)),
                KeyFrames = new DoubleKeyFrameCollection()
                {
                    new SplineDoubleKeyFrame(1, KeyTime.FromPercent(0)),
                    new EasingDoubleKeyFrame(0, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(200))) { EasingFunction = new QuarticEase() { EasingMode = EasingMode.EaseIn } },
                    new SplineDoubleKeyFrame(1, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(201)))
                }
            };
            Storyboard.SetTargetName(scaleYAnim, "IconSync");
            Storyboard.SetTargetProperty(scaleYAnim, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleY)"));


            var visSyncIconAnim = new ObjectAnimationUsingKeyFrames()
            {
                Duration = new Duration(TimeSpan.FromMilliseconds(200)),
                KeyFrames = new ObjectKeyFrameCollection()
                {
                    new DiscreteObjectKeyFrame(Visibility.Hidden, KeyTime.FromPercent(1))
                }
            };
            Storyboard.SetTargetName(visSyncIconAnim, "IconSync");
            Storyboard.SetTargetProperty(visSyncIconAnim, new PropertyPath("(UIElement.Visibility)"));


            var visSyncCompleteIconAnim = new ObjectAnimationUsingKeyFrames()
            {
                Duration = new Duration(TimeSpan.FromMilliseconds(200)),
                KeyFrames = new ObjectKeyFrameCollection()
                {
                    new DiscreteObjectKeyFrame(Visibility.Visible, KeyTime.FromPercent(1))
                }
            };
            Storyboard.SetTargetName(visSyncCompleteIconAnim, "IconSyncComplete");
            Storyboard.SetTargetProperty(visSyncCompleteIconAnim, new PropertyPath("(UIElement.Visibility)"));


            var blackstopAnim = new DoubleAnimationUsingKeyFrames()
            {
                Duration = new Duration(TimeSpan.FromMilliseconds(750)),
                KeyFrames = new DoubleKeyFrameCollection()
                {
                    new SplineDoubleKeyFrame(0, KeyTime.FromPercent(0)),
                    new SplineDoubleKeyFrame(0.33, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(350))),
                    new EasingDoubleKeyFrame(1, KeyTime.FromPercent(1)) { EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseOut } }
                }
            };
            Storyboard.SetTargetName(blackstopAnim, "BlackStop");
            Storyboard.SetTargetProperty(blackstopAnim, new PropertyPath("Offset"));


            var transparentstopAnim = new DoubleAnimationUsingKeyFrames()
            {
                Duration = new Duration(TimeSpan.FromMilliseconds(750)),
                KeyFrames = new DoubleKeyFrameCollection()
                {
                    new SplineDoubleKeyFrame(0, KeyTime.FromPercent(0)),
                    new SplineDoubleKeyFrame(0.33, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(350))),
                    new EasingDoubleKeyFrame(1, KeyTime.FromPercent(1)) { EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseOut } }
                }
            };
            Storyboard.SetTargetName(transparentstopAnim, "TransparentStop");
            Storyboard.SetTargetProperty(transparentstopAnim, new PropertyPath("Offset"));


            var fadeAnim = new DoubleAnimationUsingKeyFrames()
            {
                Duration = new Duration(TimeSpan.FromMilliseconds(3000)),
                KeyFrames = new DoubleKeyFrameCollection()
                {
                    new SplineDoubleKeyFrame(1, KeyTime.FromPercent(0)),
                    new SplineDoubleKeyFrame(1, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(1500))),
                    new SplineDoubleKeyFrame(0, KeyTime.FromPercent(1)),
                }
            };
            Storyboard.SetTargetName(fadeAnim, "IconSyncComplete");
            Storyboard.SetTargetProperty(fadeAnim, new PropertyPath("Opacity"));

            return new Storyboard()
            {
                Children = new TimelineCollection()
                {
                    scaleXAnim, scaleYAnim, visSyncIconAnim, visSyncCompleteIconAnim, blackstopAnim, transparentstopAnim, fadeAnim
                }
            };
        }
    }
}
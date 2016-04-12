using NotedUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace NotedUI.AttachedBehaviors
{
    public class NoteSyncBehavior
    {
        public static readonly DependencyProperty SyncStateProperty =
            DependencyProperty.RegisterAttached("SyncState",
                                                typeof(eNoteState),
                                                typeof(NoteSyncBehavior),
                                                new PropertyMetadata(eNoteState.Normal, new PropertyChangedCallback(SyncStateChanged)));

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
            FrameworkElement NoteSync = (FrameworkElement)d;
            eNoteState oldState = (eNoteState)e.OldValue;
            eNoteState newState = (eNoteState)e.NewValue;

            if (oldState == eNoteState.Normal && newState == eNoteState.Changed)
            {
                var sb = GetChangedStoryboard(d);

                Storyboard.SetTarget(sb, d);
                sb.Begin();
            }
            else if (oldState == eNoteState.Changed && newState == eNoteState.Syncing)
            {
                var sb = GetStartSyncingStoryboard(d);

                if (sb.IsSealed || sb.IsFrozen)
                    sb = sb.Clone();

                sb.Completed += (_, __) =>
                {
                    RunSyncingStoryboard(d);
                };
            }
        }

        private static void RunSyncingStoryboard(DependencyObject d)
        {
            var sb = GetSyncingStoryboard(d);

            if (sb.IsSealed || sb.IsFrozen)
                sb = sb.Clone();

            sb.Completed += (_, __) =>
            {
                var state = GetSyncState(d);

                if (state == eNoteState.Syncing)
                    RunSyncingStoryboard(d);

                else if (state == eNoteState.SyncComplete)
                    RunSyncCompleteStoryboard(d);
            };

            Storyboard.SetTarget(sb, d);
            sb.Begin();
        }

        private static void RunSyncCompleteStoryboard(DependencyObject d)
        {
            var sb = GetSyncCompleteStoryboard(d);

            if (sb.IsSealed || sb.IsFrozen)
                sb = sb.Clone();

            sb.Completed += (_, __) =>
            {
                SetSyncState(d, eNoteState.Normal);
            };

            Storyboard.SetTarget(sb, d);
            sb.Begin();
        }

        public static readonly DependencyProperty ChangedStoryboardProperty =
            DependencyProperty.RegisterAttached("ChangedStoryboard",
                                                typeof(Storyboard),
                                                typeof(NoteSyncBehavior),
                                                new PropertyMetadata(null));

        public static Storyboard GetChangedStoryboard(DependencyObject obj)
        {
            return (Storyboard)obj.GetValue(ChangedStoryboardProperty);
        }

        public static void SetChangedStoryboard(DependencyObject obj, Storyboard value)
        {
            obj.SetValue(ChangedStoryboardProperty, value);
        }

        public static readonly DependencyProperty StartSyncingStoryboardProperty =
            DependencyProperty.RegisterAttached("StartSyncingStoryboard",
                                                typeof(Storyboard),
                                                typeof(NoteSyncBehavior),
                                                new PropertyMetadata(null));

        public static Storyboard GetStartSyncingStoryboard(DependencyObject obj)
        {
            return (Storyboard)obj.GetValue(StartSyncingStoryboardProperty);
        }

        public static void SetStartSyncingStoryboard(DependencyObject obj, Storyboard value)
        {
            obj.SetValue(StartSyncingStoryboardProperty, value);
        }

        public static readonly DependencyProperty SyncingStoryboardProperty =
            DependencyProperty.RegisterAttached("SyncingStoryboard",
                                                typeof(Storyboard),
                                                typeof(NoteSyncBehavior),
                                                new PropertyMetadata(null));

        public static Storyboard GetSyncingStoryboard(DependencyObject obj)
        {
            return (Storyboard)obj.GetValue(SyncingStoryboardProperty);
        }

        public static void SetSyncingStoryboard(DependencyObject obj, Storyboard value)
        {
            obj.SetValue(SyncingStoryboardProperty, value);
        }

        public static readonly DependencyProperty SyncCompleteStoryboardProperty =
            DependencyProperty.RegisterAttached("SyncCompleteStoryboard",
                                                typeof(Storyboard),
                                                typeof(NoteSyncBehavior),
                                                new PropertyMetadata(null));

        public static Storyboard GetSyncCompleteStoryboard(DependencyObject obj)
        {
            return (Storyboard)obj.GetValue(SyncCompleteStoryboardProperty);
        }

        public static void SetSyncCompleteStoryboard(DependencyObject obj, Storyboard value)
        {
            obj.SetValue(SyncCompleteStoryboardProperty, value);
        }
    }
}
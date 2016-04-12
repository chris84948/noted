using NotedUI.UI.Screens;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace NotedUI.AttachedBehaviors
{
    public class ItemRemovalBehavior
    {
        public static readonly DependencyProperty StoryboardProperty =
            DependencyProperty.RegisterAttached(
                "Storyboard",
                typeof(Storyboard),
                typeof(ItemRemovalBehavior),
                null);

        public static Storyboard GetStoryboard(DependencyObject o)
        {
            return o.GetValue(StoryboardProperty) as Storyboard;
        }

        public static void SetStoryboard(DependencyObject o, Storyboard value)
        {
            o.SetValue(StoryboardProperty, value);
        }

        public static readonly DependencyProperty PerformRemovalProperty =
            DependencyProperty.RegisterAttached(
                "PerformRemoval",
                typeof(ICommand),
                typeof(ItemRemovalBehavior),
                null);

        public static ICommand GetPerformRemoval(DependencyObject o)
        {
            return o.GetValue(PerformRemovalProperty) as ICommand;
        }

        public static void SetPerformRemoval(DependencyObject o, ICommand value)
        {
            o.SetValue(PerformRemovalProperty, value);
        }

        public static readonly DependencyProperty IsMarkedForRemovalProperty =
            DependencyProperty.RegisterAttached(
                "IsMarkedForRemoval",
                typeof(bool),
                typeof(ItemRemovalBehavior),
                new UIPropertyMetadata(false, OnMarkedForRemovalChanged));

        public static bool GetIsMarkedForRemoval(DependencyObject o)
        {
            return o.GetValue(IsMarkedForRemovalProperty) as bool? ?? false;
        }

        public static void SetIsMarkedForRemoval(DependencyObject o, bool value)
        {
            o.SetValue(IsMarkedForRemovalProperty, value);
        }

        private static void OnMarkedForRemovalChanged(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            if ((e.NewValue as bool?) != true)
                return;

            var element = d as ListViewItem;
            if (element == null)
                throw new InvalidOperationException(
                    "MarkedForRemoval can only be set on a FrameworkElement");

            var performRemoval = GetPerformRemoval(d);
            if (performRemoval == null)
                throw new InvalidOperationException(
                    "MarkedForRemoval requires PerformRemoval to be set too");

            var sb = GetStoryboard(d);
            if (sb == null)
                throw new InvalidOperationException(
                    "MarkedForRemoval requires Stoyboard to be set too");

            var listView = FindUpVisualTree<ListView>(element);
            var grid = VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(element, 0), 0) as Grid;
            grid.Width = listView.ActualWidth;
            grid.HorizontalAlignment = HorizontalAlignment.Center;
            grid.VerticalAlignment = VerticalAlignment.Center;

            if (sb.IsSealed || sb.IsFrozen)
                sb = sb.Clone();

            Storyboard.SetTarget(sb, d);
            sb.Completed += (_, __) =>
            {
                var data = new ListData((ObservableCollection<Note>)listView.ItemsSource, 
                                        (Note)element.DataContext);
                
                if (!performRemoval.CanExecute(data))
                    return;

                performRemoval.Execute(data);
            };

            sb.Begin();
        }

        // walk up the visual tree to find object of type T, starting from initial object
        public static T FindUpVisualTree<T>(DependencyObject initial) where T : DependencyObject
        {
            DependencyObject current = initial;

            while (current != null && current.GetType() != typeof(T))
            {
                current = VisualTreeHelper.GetParent(current);
            }
            return current as T;
        }
    }
}

using NotedUI.Models;
using NotedUI.UI.Screens;
using NotedUI.UI.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace NotedUI.AttachedBehaviors
{
    public class ListViewItemBehavior
    {
        // Attached properties/events for removal animation
        public static readonly DependencyProperty RemoveStoryboardProperty =
            DependencyProperty.RegisterAttached(
                "RemoveStoryboard",
                typeof(Storyboard),
                typeof(ListViewItemBehavior),
                null);

        public static Storyboard GetRemoveStoryboard(DependencyObject o)
        {
            return o.GetValue(RemoveStoryboardProperty) as Storyboard;
        }

        public static void SetRemoveStoryboard(DependencyObject o, Storyboard value)
        {
            o.SetValue(RemoveStoryboardProperty, value);
        }

        public static readonly DependencyProperty PerformRemovalProperty =
            DependencyProperty.RegisterAttached(
                "PerformRemoval",
                typeof(ICommand),
                typeof(ListViewItemBehavior),
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
                typeof(ListViewItemBehavior),
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

            var sb = GetRemoveStoryboard(d);
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
                var data = new ListData((ObservableCollection<NoteViewModel>)(listView.ItemsSource as ListCollectionView).SourceCollection, 
                                        (NoteViewModel)element.DataContext);
                
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


        // Attached Properties/Events for Animating list view item on addition
        public static readonly DependencyProperty AnimateOnAddProperty =
            DependencyProperty.RegisterAttached("AnimateOnAdd",
                                                typeof(bool),
                                                typeof(ListViewItemBehavior),
                                                new PropertyMetadata(false, new PropertyChangedCallback(AnimateOnAddChanged)));

        public static bool GetAnimateOnAdd(DependencyObject obj)
        {
            return (bool)obj.GetValue(AnimateOnAddProperty);
        }

        public static void SetAnimateOnAdd(DependencyObject obj, bool value)
        {
            obj.SetValue(AnimateOnAddProperty, value);
        }

        private static void AnimateOnAddChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement listviewItem = (FrameworkElement)d;
            
            if ((bool)e.NewValue)
            {
                listviewItem.Loaded += (sender, args) =>
                {
                    ListViewItem item = sender as ListViewItem;
                    NoteViewModel dataContext = (NoteViewModel)item.DataContext;

                    if (dataContext.AnimateOnLoad)
                    {
                        // Clear this flag, so it only happens once then show the storyboard
                        dataContext.AnimateOnLoad = false;

                        var sb = GetAdditionStoryboard(d);
                        if (sb.IsSealed || sb.IsFrozen)
                            sb = sb.Clone();

                        Storyboard.SetTarget(sb, d);
                        sb.Begin();
                    }
                };
            }
        }

        public static readonly DependencyProperty AdditionStoryboardProperty =
            DependencyProperty.RegisterAttached("AdditionStoryboard",
                                                typeof(Storyboard),
                                                typeof(ListViewItemBehavior),
                                                new PropertyMetadata(null));

        public static Storyboard GetAdditionStoryboard(DependencyObject obj)
        {
            return (Storyboard)obj.GetValue(AdditionStoryboardProperty);
        }

        public static void SetAdditionStoryboard(DependencyObject obj, Storyboard value)
        {
            obj.SetValue(AdditionStoryboardProperty, value);
        }
    }
}

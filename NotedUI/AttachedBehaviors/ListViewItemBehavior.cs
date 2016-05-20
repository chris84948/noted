using NotedUI.UI.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace NotedUI.AttachedBehaviors
{
    public class ListViewItemBehavior
    {
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

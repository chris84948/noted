using NotedUI.UI.ViewModels;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace NotedUI.Controls
{
    public class NotedExpander : Expander
    {
        private bool _eventsInitialized = false;

        public static readonly DependencyProperty ExpandNowProperty =
            DependencyProperty.RegisterAttached("ExpandNow",
                                                typeof(bool),
                                                typeof(NotedExpander),
                                                new PropertyMetadata(false, new PropertyChangedCallback(ExpandNowChanged)));

        public static bool GetExpandNow(DependencyObject obj)
        {
            return (bool)obj.GetValue(ExpandNowProperty);
        }

        public static void SetExpandNow(DependencyObject obj, bool value)
        {
            obj.SetValue(ExpandNowProperty, value);
        }

        public static readonly DependencyProperty ShowHighlightProperty =
            DependencyProperty.RegisterAttached("ShowHighlight",
                                                typeof(bool),
                                                typeof(NotedExpander),
                                                new PropertyMetadata(false));

        public static bool GetShowHighlight(DependencyObject obj)
        {
            return (bool)obj.GetValue(ShowHighlightProperty);
        }

        public static void SetShowHighlight(DependencyObject obj, bool value)
        {
            obj.SetValue(ShowHighlightProperty, value);
        }

        static NotedExpander()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NotedExpander), new FrameworkPropertyMetadata(typeof(NotedExpander)));
            ContentProperty.OverrideMetadata(typeof(NotedExpander), new FrameworkPropertyMetadata(new PropertyChangedCallback(OnContentChangedCallback)));
        }

        private static void OnContentChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var expander = d as NotedExpander;
            ListView listview = GetListViewParent(expander);

            if (listview == null || expander?._eventsInitialized == true)
                return;

            ((INotifyCollectionChanged)listview.Items).CollectionChanged += (sender, args) =>
            {
                // If a new item is added, make sure the expander is open
                expander.IsExpanded = true;
            };

            // First run, open the expander and select the new item - this only runs when the group is first added
            if (expander.IsLoaded)
                expander.IsExpanded = true;
            expander._eventsInitialized = true;
        }

        public NotedExpander()
        {
            DragEnter += Expander_DragEnter;
            DragLeave += Expander_DragLeave;
            Drop += Expander_DragLeave;
        }

        private static void ExpandNowChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(bool)e.NewValue)
                return;

            Expander expander = (Expander)d;

            if (!expander.IsExpanded)
                expander.IsExpanded = true;

            // Reset the value for next time
            SetExpandNow(d, false);
        }

        private static ListView GetListViewParent(DependencyObject control)
        {
            var parent = VisualTreeHelper.GetParent(control);

            while (parent != null && !(parent is ListView))
                parent = VisualTreeHelper.GetParent(parent);

            if (parent is ListView)
                return parent as ListView;
            else
                return null;
        }

        private static void Expander_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent("GongSolutions.Wpf.DragDrop"))
                return;

            var draggedNote = e.Data.GetData("GongSolutions.Wpf.DragDrop") as NoteViewModel;
            var expander = sender as Expander;

            if (draggedNote.Group.ToUpper() != expander.Header.ToString().ToUpper())
                SetShowHighlight(expander, true);
        }

        private static void Expander_DragLeave(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent("GongSolutions.Wpf.DragDrop"))
                return;

            var draggedNote = e.Data.GetData("GongSolutions.Wpf.DragDrop") as NoteViewModel;
            var expander = sender as Expander;

            if (draggedNote.Group.ToUpper() != expander.Header.ToString().ToUpper())
                SetShowHighlight(expander, false);
        }
    }
}

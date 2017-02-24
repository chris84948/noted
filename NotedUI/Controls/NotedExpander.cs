using NotedUI.DataStorage;
using NotedUI.UI.ViewModels;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
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

        public static readonly DependencyProperty LocalStorageProperty =
            DependencyProperty.RegisterAttached("LocalStorage",
                                                typeof(ILocalStorage),
                                                typeof(NotedExpander),
                                                new PropertyMetadata(null));

        public static ILocalStorage GetLocalStorage(DependencyObject obj)
        {
            return (ILocalStorage)obj.GetValue(LocalStorageProperty);
        }

        public static void SetLocalStorage(DependencyObject obj, ILocalStorage value)
        {
            obj.SetValue(LocalStorageProperty, value);
        }

        private static void LocalStorageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement LocalStorage = (FrameworkElement)d;
            double newVal = (double)e.NewValue;
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
                if (args.NewItems == null)
                    return;

                // If a new item is added, make sure the expander is open
                if (expander.Header.ToString() == (args?.NewItems[0] as NoteViewModel).Group)
                    expander.IsExpanded = true;
            };

            listview.SelectionChanged += (sender, args) =>
            {
                if (listview.SelectedItem == null)
                    return;

                // Make sure the expander is selected if this item is selected
                if (expander.Header.ToString() == (listview.SelectedItem as NoteViewModel).Group)
                    expander.IsExpanded = true;
            };

            // Make sure on startup that the selected item is expanded
            // TODO turned off selected note expanded code for now
            var expanderItems = ((expander.Content as ItemsPresenter).DataContext as CollectionViewGroup).Items;
            if (expanderItems.Contains(listview.SelectedItem))
                expander.IsExpanded = true;

            expander._eventsInitialized = true;
        }

        public NotedExpander()
        {
            DragEnter += Expander_DragEnter;
            DragLeave += Expander_DragLeave;
            Drop += Expander_DragLeave;
            Loaded += NotedExpander_Loaded;
        }

        private async void NotedExpander_Loaded(object sender, RoutedEventArgs e)
        {
            ILocalStorage localStorage = GetLocalStorage(this);

            if (localStorage == null)
                return;

            // Get the stored value of is expanded and expand it if it's not already
            IsExpanded = IsExpanded || await localStorage.IsGroupExpanded(Header.ToString().ToUpper());

            Expanded += NotedExpander_Expanded;
            Collapsed += NotedExpander_Collapsed;
        }

        private void NotedExpander_Expanded(object sender, RoutedEventArgs e)
        {
            ILocalStorage localStorage = GetLocalStorage(this);
            localStorage?.InsertExpandedGroup(Header.ToString().ToUpper());
        }

        private void NotedExpander_Collapsed(object sender, RoutedEventArgs e)
        {
            ILocalStorage localStorage = GetLocalStorage(this);
            localStorage?.DeleteExpandedGroup(Header.ToString().ToUpper());
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

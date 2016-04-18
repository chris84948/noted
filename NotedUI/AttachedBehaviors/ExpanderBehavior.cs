using NotedUI.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace NotedUI.AttachedBehaviors
{
    public static class ExpanderBehavior
    {
        public static readonly DependencyProperty HighlightOnDragProperty =
            DependencyProperty.RegisterAttached("HighlightOnDrag",
                                                typeof(bool),
                                                typeof(ExpanderBehavior),
                                                new PropertyMetadata(false, new PropertyChangedCallback(HighlightOnDragChanged)));

        public static bool GetHighlightOnDrag(DependencyObject obj)
        {
            return (bool)obj.GetValue(HighlightOnDragProperty);
        }

        public static void SetHighlightOnDrag(DependencyObject obj, bool value)
        {
            obj.SetValue(HighlightOnDragProperty, value);
        }

        public static readonly DependencyProperty ShowHighlightProperty =
            DependencyProperty.RegisterAttached("ShowHighlight",
                                                typeof(bool),
                                                typeof(ExpanderBehavior),
                                                new PropertyMetadata(false));

        public static bool GetShowHighlight(DependencyObject obj)
        {
            return (bool)obj.GetValue(ShowHighlightProperty);
        }

        public static void SetShowHighlight(DependencyObject obj, bool value)
        {
            obj.SetValue(ShowHighlightProperty, value);
        }

        private static void HighlightOnDragChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Expander expander = (Expander)d;

            if (!(bool)e.NewValue)
                return;

            expander.DragEnter += Expander_DragEnter;
            expander.DragLeave += Expander_DragLeave;
        }

        private static void Expander_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent("GongSolutions.Wpf.DragDrop"))
                return;

            var draggedNote = e.Data.GetData("GongSolutions.Wpf.DragDrop") as NoteViewModel;
            var expander = sender as Expander;

            if (draggedNote.Folder.ToUpper() != expander.Header.ToString().ToUpper())
                SetShowHighlight(expander, true);
        }

        private static void Expander_DragLeave(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent("GongSolutions.Wpf.DragDrop"))
                return;

            var draggedNote = e.Data.GetData("GongSolutions.Wpf.DragDrop") as NoteViewModel;
            var expander = sender as Expander;

            if (draggedNote.Folder.ToUpper() != expander.Header.ToString().ToUpper())
                SetShowHighlight(expander, false);
        }
    }
}

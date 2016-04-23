using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NotedUI.AttachedBehaviors
{
    class FocusBehavior
    {
        public static readonly DependencyProperty FocusOnLoadedProperty =
            DependencyProperty.RegisterAttached("FocusOnLoaded",
                                                typeof(bool),
                                                typeof(FocusBehavior),
                                                new PropertyMetadata(false, new PropertyChangedCallback(FocusOnLoadedChanged)));

        public static bool GetFocusOnLoaded(DependencyObject obj)
        {
            return (bool)obj.GetValue(FocusOnLoadedProperty);
        }

        public static void SetFocusOnLoaded(DependencyObject obj, bool value)
        {
            obj.SetValue(FocusOnLoadedProperty, value);
        }

        private static void FocusOnLoadedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement FocusOnLoaded = (FrameworkElement)d;
            bool newVal = (bool)e.NewValue;

            if (!newVal)
                return;

            FocusOnLoaded.Focusable = true;
            FocusOnLoaded.Loaded += (s, args) => FocusOnLoaded.Focus();
        }
    }
}

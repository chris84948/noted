using ICSharpCode.AvalonEdit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interactivity;

namespace NotedUI.AttachedBehaviors
{
    public static class AvalonEditBehaviour
    {
        public static readonly DependencyProperty BindToTextProperty =
            DependencyProperty.RegisterAttached("BindToText",
                                                typeof(bool),
                                                typeof(AvalonEditBehaviour),
                                                new PropertyMetadata(false, new PropertyChangedCallback(BindToTextChanged)));

        public static bool GetBindToText(DependencyObject obj)
        {
            return (bool)obj.GetValue(BindToTextProperty);
        }

        public static void SetBindToText(DependencyObject obj, bool value)
        {
            obj.SetValue(BindToTextProperty, value);
        }

        private static void BindToTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TextEditor editor = (TextEditor)d;
            bool newVal = (bool)e.NewValue;

            if (!newVal)
                return;

            editor.TextChanged += (sender, args) =>
            {
                SetBindableText(d, editor.Text);
            };
        }

        public static readonly DependencyProperty BindableTextProperty =
            DependencyProperty.RegisterAttached("BindableText",
                                                typeof(string),
                                                typeof(AvalonEditBehaviour),
                                                new PropertyMetadata(null));

        public static string GetBindableText(DependencyObject obj)
        {
            return (string)obj.GetValue(BindableTextProperty);
        }

        public static void SetBindableText(DependencyObject obj, string value)
        {
            obj.SetValue(BindableTextProperty, value);
        }
    }
}

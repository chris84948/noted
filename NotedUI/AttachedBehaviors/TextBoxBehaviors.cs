using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace NotedUI.AttachedBehaviors
{
    public static class TextBoxBehaviors
    {
        public static readonly DependencyProperty HintProperty = DependencyProperty.RegisterAttached(
            "Hint",
            typeof(string),
            typeof(TextBoxBehaviors),
            new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.Inherits));

        public static readonly DependencyProperty TextBoxViewMarginProperty = DependencyProperty.RegisterAttached(
            "TextBoxViewMargin",
            typeof(Thickness),
            typeof(TextBoxBehaviors),
            new PropertyMetadata(new Thickness(double.NegativeInfinity), TextBoxViewMarginPropertyChangedCallback));

        public static readonly DependencyProperty TextProperty = DependencyProperty.RegisterAttached(
            "Text",
            typeof(string),
            typeof(TextBoxBehaviors),
            new PropertyMetadata(default(string), TextPropertyChangedCallback));

        private static readonly DependencyPropertyKey IsNullOrEmptyPropertyKey = DependencyProperty.RegisterAttachedReadOnly(
            "IsNullOrEmpty",
            typeof(bool),
            typeof(TextBoxBehaviors),
            new PropertyMetadata(true));
        public static readonly DependencyProperty IsNullOrEmptyProperty = IsNullOrEmptyPropertyKey.DependencyProperty;

        public static readonly DependencyProperty ClearTextButtonProperty = DependencyProperty.RegisterAttached(
            "ClearTextButton",
            typeof(bool),
            typeof(TextBoxBehaviors),
            new FrameworkPropertyMetadata(false));

        public static readonly DependencyProperty IsClearTextButtonBehaviorEnabledProperty = DependencyProperty.RegisterAttached(
            "IsClearTextButtonBehaviorEnabled",
            typeof(bool),
            typeof(TextBoxBehaviors),
            new FrameworkPropertyMetadata(false, IsClearTextButtonBehaviorEnabledChanged));
        
        public static string GetHint(DependencyObject element)
        {
            return (string)element.GetValue(HintProperty);
        }
        public static void SetHint(DependencyObject element, string value)
        {
            element.SetValue(HintProperty, value);
        }

        public static Thickness GetTextBoxViewMargin(DependencyObject element)
        {
            return (Thickness)element.GetValue(TextBoxViewMarginProperty);
        }
        public static void SetTextBoxViewMargin(DependencyObject element, Thickness value)
        {
            element.SetValue(TextBoxViewMarginProperty, value);
        }

        public static void SetText(DependencyObject element, string value)
        {
            element.SetValue(TextProperty, value);
        }
        public static string GetText(DependencyObject element)
        {
            return (string)element.GetValue(TextProperty);
        }

        public static bool GetClearTextButton(DependencyObject d)
        {
            return (bool)d.GetValue(ClearTextButtonProperty);
        }
        public static void SetClearTextButton(DependencyObject obj, bool value)
        {
            obj.SetValue(ClearTextButtonProperty, value);
        }

        [AttachedPropertyBrowsableForType(typeof(Button))]
        public static bool GetIsClearTextButtonBehaviorEnabled(Button d)
        {
            return (bool)d.GetValue(IsClearTextButtonBehaviorEnabledProperty);
        }
        [AttachedPropertyBrowsableForType(typeof(Button))]
        public static void SetIsClearTextButtonBehaviorEnabled(Button obj, bool value)
        {
            obj.SetValue(IsClearTextButtonBehaviorEnabledProperty, value);
        }

        private static void SetIsNullOrEmpty(DependencyObject element, bool value)
        {
            element.SetValue(IsNullOrEmptyPropertyKey, value);
        }
        public static bool GetIsNullOrEmpty(DependencyObject element)
        {
            return (bool)element.GetValue(IsNullOrEmptyProperty);
        }

        private static void TextBoxViewMarginPropertyChangedCallback(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var box = dependencyObject as Control; //could be a text box or password box
            if (box == null)
            {
                return;
            }

            if (box.IsLoaded)
            {
                ApplyTextBoxViewMargin(box, (Thickness)dependencyPropertyChangedEventArgs.NewValue);
            }

            box.Loaded += (sender, args) =>
            {
                var textBox = (Control)sender;
                ApplyTextBoxViewMargin(textBox, GetTextBoxViewMargin(textBox));
            };
        }

        private static void ApplyTextBoxViewMargin(Control textBox, Thickness margin)
        {
            if (margin.Equals(new Thickness(double.NegativeInfinity)))
            {
                return;
            }

            var scrollViewer = textBox.Template.FindName("PART_ContentHost", textBox) as ScrollViewer;
            if (scrollViewer == null)
            {
                return;
            }

            var frameworkElement = scrollViewer.Content as FrameworkElement;

            if (frameworkElement != null)
            {
                frameworkElement.Margin = margin;
            }
        }

        private static void TextPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            SetIsNullOrEmpty(dependencyObject, string.IsNullOrEmpty((dependencyPropertyChangedEventArgs.NewValue ?? "").ToString()));
        }

        private static void IsClearTextButtonBehaviorEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var button = d as Button;
            if (e.OldValue != e.NewValue && button != null)
            {
                button.Click -= ButtonClicked;
                if ((bool)e.NewValue)
                {
                    button.Click += ButtonClicked;
                }
            }
        }

        public static void ButtonClicked(object sender, RoutedEventArgs e)
        {
            var button = ((Button)sender);
            var parent = VisualTreeHelper.GetParent(button);
            while (!(parent is TextBox || parent is PasswordBox || parent is ComboBox))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }

            if (parent is TextBox)
            {
                var tb = parent as TextBox;
                tb.Clear();
                tb.Focus();
                tb.Select(0, 0);
            }
            else if (parent is PasswordBox)
            {
                ((PasswordBox)parent).Clear();
            }
            else if (parent is ComboBox)
            {
                if (((ComboBox)parent).IsEditable)
                {
                    ((ComboBox)parent).Text = string.Empty;
                }
                ((ComboBox)parent).SelectedItem = null;
            }
        }

        public static readonly DependencyProperty DoubleClickSelectAllProperty =
            DependencyProperty.RegisterAttached("DoubleClickSelectAll",
                                                typeof(bool),
                                                typeof(TextBoxBehaviors),
                                                new PropertyMetadata(false, new PropertyChangedCallback(DoubleClickSelectAllChanged)));

        public static bool GetDoubleClickSelectAll(DependencyObject obj)
        {
            return (bool)obj.GetValue(DoubleClickSelectAllProperty);
        }

        public static void SetDoubleClickSelectAll(DependencyObject obj, bool value)
        {
            obj.SetValue(DoubleClickSelectAllProperty, value);
        }

        private static void DoubleClickSelectAllChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TextBox tb = (TextBox)d;

            if (!(bool)e.NewValue)
                return;

            tb.MouseDoubleClick += (sender, args) =>
            {
                tb.SelectAll();
            };
        }
    }
}

using ICSharpCode.AvalonEdit;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interactivity;

namespace NotedUI.AttachedBehaviors
{
    public sealed class AvalonEditBehaviour : Behavior<TextEditor>
    {
        public static readonly DependencyProperty BindableTextProperty =
            DependencyProperty.Register("BindableText",
                                        typeof(string),
                                        typeof(AvalonEditBehaviour),
                                        new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnBindableTextChanged)));

        public string BindableText
        {
            get { return (string)GetValue(BindableTextProperty); }
            set { SetValue(BindableTextProperty, value); }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            if (AssociatedObject != null)
                AssociatedObject.TextChanged += AssociatedObjectOnTextChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            if (AssociatedObject != null)
                AssociatedObject.TextChanged -= AssociatedObjectOnTextChanged;
        }

        private void AssociatedObjectOnTextChanged(object sender, EventArgs eventArgs)
        {
            var textEditor = sender as TextEditor;
            if (textEditor != null)
            {
                if (textEditor.Document != null)
                    BindableText = textEditor.Document.Text;
            }
        }

        private static void OnBindableTextChanged(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var behavior = dependencyObject as AvalonEditBehaviour;
            if (behavior.AssociatedObject != null)
            {
                var editor = behavior.AssociatedObject as TextEditor;
                if (editor.Document != null && dependencyPropertyChangedEventArgs.NewValue != null)
                {
                    editor.Document.Text = dependencyPropertyChangedEventArgs.NewValue.ToString();

                    Task.Run(() =>
                    {
                        // Delay to let whatever Avalon does to unfocus, do it's thing.
                        // If i remove this task, the highlight stuff doesn't work
                        System.Threading.Thread.Sleep(10);

                        App.Current.Dispatcher.Invoke(() =>
                        {
                            editor.Focus();
                            editor.Select(editor.Document.Text.Length, 0);
                        });
                    });
                }
            }
        }
    }
}

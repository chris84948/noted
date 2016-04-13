using System.Windows;
using System.Windows.Media;

namespace NotedUI.AttachedBehaviors
{
    public class ButtonBehaviors : DependencyObject
    {
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(Visual), typeof(ButtonBehaviors), new PropertyMetadata(null));

        public static readonly DependencyProperty IconSizeProperty =
            DependencyProperty.Register("IconSize", typeof(double), typeof(ButtonBehaviors),
                new FrameworkPropertyMetadata(0.0));

        public static Visual GetIcon(DependencyObject obj)
        {
            return (Visual)obj.GetValue(IconProperty);
        }

        public static void SetIcon(DependencyObject obj, Visual value)
        {
            obj.SetValue(IconProperty, value);
        }

        public static double GetIconSize(DependencyObject obj)
        {
            return (double)obj.GetValue(IconSizeProperty);
        }

        public static void SetIconSize(DependencyObject obj, double value)
        {
            obj.SetValue(IconSizeProperty, value);
        }
    }
}

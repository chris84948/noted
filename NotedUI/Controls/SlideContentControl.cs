using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NotedUI.Controls
{
    public class SlideContentControl : ContentControl
    {
        static SlideContentControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SlideContentControl), new FrameworkPropertyMetadata(typeof(SlideContentControl)));

            ContentProperty.OverrideMetadata(typeof(SlideContentControl), new FrameworkPropertyMetadata(null, ContentHasChanged));
        }

        private static object ContentHasChanged(DependencyObject d, object baseValue)
        {
            throw new NotImplementedException();
        }
    }
}

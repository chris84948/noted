using System;
using System.Windows;
using System.Windows.Controls;

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

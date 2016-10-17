using CommonMark;
using Markdig;
using System;
using System.Globalization;
using System.Windows.Data;

namespace NotedUI.Converters
{
    [ValueConversion(typeof(string), typeof(string))]
    public class MarkdownConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is string))
                return "";

            //return CommonMarkConverter.Convert(value.ToString());
            var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
            return Markdown.ToHtml(value.ToString(), pipeline);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}

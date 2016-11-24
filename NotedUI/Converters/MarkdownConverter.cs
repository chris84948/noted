using CommonMark;
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

            return CommonMarkConverter.Convert(value.ToString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}

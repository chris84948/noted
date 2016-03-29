using System;
using System.Globalization;
using System.Windows.Data;

namespace NotedUI.Converters
{
    [ValueConversion(typeof(double), typeof(double))]
    public class InvertValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double)
                return -(double)value;

            else if (value is bool)
                return !(bool)value;

            else if (value is int)
                return -(int)value;

            else
                return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}

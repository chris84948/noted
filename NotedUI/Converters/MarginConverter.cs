using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace NotedUI.Converters
{
    [ValueConversion(typeof(double), typeof(Thickness))]
    public class MarginConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is double))
                return 0;

            return new Thickness(-(double)value * 2, 0, 0, 0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}

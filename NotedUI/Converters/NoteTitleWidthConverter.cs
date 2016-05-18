using System;
using System.Globalization;
using System.Windows.Data;

namespace NotedUI.Converters
{
    [ValueConversion(typeof(double), typeof(double))]
    public class NoteTitleWidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is double))
                return 1;

            return (double)value - 75; // Width of margins and icons in listview item
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}

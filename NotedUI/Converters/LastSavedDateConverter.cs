using System;
using System.Globalization;
using System.Windows.Data;

namespace NotedUI.Converters
{
    [ValueConversion(typeof(DateTime), typeof(string))]
    public class LastSavedDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is DateTime))
                return "";

            var lastSaved = (DateTime)value;
            var now = DateTime.Now;

            if (lastSaved > now.AddHours(-1))
                return GetTimeString((now - lastSaved).Minutes, "minute");

            else if (lastSaved > now.AddDays(-1))
                return GetTimeString((now - lastSaved).Hours, "hour");

            else if (lastSaved > now.AddMonths(-1))
                return GetTimeString((now - lastSaved).Days, "day");

            else
                return lastSaved;
        }

        private string GetTimeString(int num, string timeUnit)
        {
            if (num == 1)
                return $"{ num } { timeUnit } ago";
            else
                return $"{ num } { timeUnit }s ago";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

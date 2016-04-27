using ICSharpCode.AvalonEdit;
using NotedUI.Models;
using NotedUI.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace NotedUI.Converters
{
    public class DialogDataConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(values[0] is HomeViewModel) || !(values[1] is TextEditor))
                return null;

            else
                return new DialogData(values[0] as HomeViewModel, values[1] as TextEditor);
        }

        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}

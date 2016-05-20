using NotedUI.Models;
using NotedUI.UI.ViewModels;
using System;
using System.Globalization;
using System.Windows.Data;

namespace NotedUI.Converters
{
    public class ContextMenuCommandParamConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(values[0] is AllNotesViewModel) || !(values[1] is string) || !(values[2] is HomeViewModel))
                return null;

            var allNotes = values[0] as AllNotesViewModel;
            var groupName = values[1] as string;
            var homeVM = values[2] as HomeViewModel;

            return new GroupCmdParams(homeVM, allNotes, groupName);
        }

        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}

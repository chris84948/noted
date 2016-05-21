using NotedUI.Models;
using NotedUI.UI.ViewModels;
using System;
using System.Globalization;
using System.Windows.Data;

namespace NotedUI.Converters
{
    public class ContextMenuNoteCommandParamConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(values[0] is AllNotesViewModel) || !(values[1] is NoteViewModel))
                return null;

            var allNotes = values[0] as AllNotesViewModel;
            var noteToDelete = values[1] as NoteViewModel;

            return new DeleteNoteParams(allNotes, noteToDelete);
        }

        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}

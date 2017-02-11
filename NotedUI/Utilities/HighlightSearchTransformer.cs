using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace NotedUI.Utilities
{
    public class HighlightSearchTransformer : DocumentColorizingTransformer
    {
        private string _highlightText;
        private Brush _backgroundBrush;
        private Brush _foregroundBrush;

        public HighlightSearchTransformer(string text)
        {
            _highlightText = text;

            _backgroundBrush = (Brush)new BrushConverter().ConvertFromString("#FF003261");
            _foregroundBrush = Brushes.White;
            _backgroundBrush.Freeze();
            _foregroundBrush.Freeze();
        }

        protected override void ColorizeLine(DocumentLine line)
        {
            int lineStartOffset = line.Offset;
            string text = CurrentContext.Document.GetText(line);
            int start = 0;
            int index;
            while ((index = text.IndexOf(_highlightText, start, StringComparison.CurrentCultureIgnoreCase)) >= 0)
            {
                ChangeLinePart(lineStartOffset + index, // startOffset
                               lineStartOffset + index + _highlightText.Length, // endOffset
                               (VisualLineElement element) =>
                               {
                                   // This lambda gets called once for every VisualLineElement
                                   // between the specified offsets.
                                   element.TextRunProperties.SetBackgroundBrush(_backgroundBrush);
                                   element.TextRunProperties.SetForegroundBrush(_foregroundBrush);
                               });

                start = index + 1; // search for next occurrence
            }
        }
    }
}

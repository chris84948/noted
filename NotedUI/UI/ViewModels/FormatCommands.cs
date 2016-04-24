using ICSharpCode.AvalonEdit;
using JustMVVM;
using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

namespace NotedUI.UI.ViewModels
{
    public class FormatCommands : MVVMBase
    {
        public ICommand Header1Command { get { return new RelayCommand<TextEditor>(Header1Exec, CanHeader1Exec); } }
        public ICommand Header2Command { get { return new RelayCommand<TextEditor>(Header2Exec, CanHeader2Exec); } }
        public ICommand Header3Command { get { return new RelayCommand<TextEditor>(Header3Exec, CanHeader3Exec); } }
        public ICommand Header4Command { get { return new RelayCommand<TextEditor>(Header4Exec, CanHeader4Exec); } }
        public ICommand Header5Command { get { return new RelayCommand<TextEditor>(Header5Exec, CanHeader5Exec); } }
        public ICommand Header6Command { get { return new RelayCommand<TextEditor>(Header6Exec, CanHeader6Exec); } }
        public ICommand BoldCommand { get { return new RelayCommand<TextEditor>(BoldExec, CanBoldExec); } }
        public ICommand ItalicCommand { get { return new RelayCommand<TextEditor>(ItalicExec, CanItalicExec); } }
        public ICommand UnderlineCommand { get { return new RelayCommand<TextEditor>(UnderlineExec, CanUnderlineExec); } }
        public ICommand StrikethroughCommand { get { return new RelayCommand<TextEditor>(StrikethroughExec, CanStrikethroughExec); } }
        public ICommand QuotesCommand { get { return new RelayCommand<TextEditor>(QuotesExec, CanQuotesExec); } }
        public ICommand CodeCommand { get { return new RelayCommand<TextEditor>(CodeExec, CanCodeExec); } }
        public ICommand BulletPointCommand { get { return new RelayCommand<TextEditor>(BulletPointExec, CanBulletPointExec); } }
        public ICommand ListCommand { get { return new RelayCommand<TextEditor>(ListExec, CanListExec); } }
        public ICommand ImageCommand { get { return new RelayCommand<TextEditor>(ImageExec, CanImageExec); } }
        public ICommand LinkCommand { get { return new RelayCommand<TextEditor>(LinkExec, CanLinkExec); } }
        public ICommand HorizontalLineCommand { get { return new RelayCommand<TextEditor>(HorizontalLineExec, CanHorizontalLineExec); } }

        public bool CanHeader1Exec(TextEditor tbNote)
        {
            return true;
        }

        public void Header1Exec(TextEditor tbNote)
        {
            FormatText(tbNote, "# ", null);
        }

        public bool CanHeader2Exec(TextEditor tbNote)
        {
            return true;
        }

        public void Header2Exec(TextEditor tbNote)
        {
            FormatText(tbNote, "## ", null);
        }

        public bool CanHeader3Exec(TextEditor tbNote)
        {
            return true;
        }

        public void Header3Exec(TextEditor tbNote)
        {
            FormatText(tbNote, "### ", null);
        }

        public bool CanHeader4Exec(TextEditor tbNote)
        {
            return true;
        }

        public void Header4Exec(TextEditor tbNote)
        {
            FormatText(tbNote, "#### ", null);
        }

        public bool CanHeader5Exec(TextEditor tbNote)
        {
            return true;
        }

        public void Header5Exec(TextEditor tbNote)
        {
            FormatText(tbNote, "##### ", null);
        }

        public bool CanHeader6Exec(TextEditor tbNote)
        {
            return true;
        }

        public void Header6Exec(TextEditor tbNote)
        {
            FormatText(tbNote, "###### ", null);
        }

        public bool CanBoldExec(TextEditor tbNote)
        {
            return true;
        }

        public void BoldExec(TextEditor tbNote)
        {
            FormatText(tbNote, "**", "**");
        }

        public bool CanItalicExec(TextEditor tbNote)
        {
            return true;
        }

        public void ItalicExec(TextEditor tbNote)
        {
            FormatText(tbNote, "*", "*");
        }

        public bool CanUnderlineExec(TextEditor tbNote)
        {
            return true;
        }

        public void UnderlineExec(TextEditor tbNote)
        {
            FormatText(tbNote, "<u>", "</u>");
        }

        public bool CanStrikethroughExec(TextEditor tbNote)
        {
            return true;
        }

        public void StrikethroughExec(TextEditor tbNote)
        {
            FormatText(tbNote, "<s>", "</s>");
        }

        public bool CanQuotesExec(TextEditor tbNote)
        {
            return true;
        }

        public void QuotesExec(TextEditor tbNote)
        {
            if (tbNote.SelectionLength == 0)
                AddNoSelectionQuotes(tbNote);

            else
                AddSelectionQuotes(tbNote);
        }

        private void AddNoSelectionQuotes(TextEditor tbNote)
        {
            var numPreviousEmptyLines = GetNumberOfPreviousLineBreaks(tbNote);
            var startPos = tbNote.SelectionStart;
            var newText = "";

            if (tbNote.SelectionStart == 0 || numPreviousEmptyLines >= 2)
                newText = ">\r\n\r\n";

            else if (numPreviousEmptyLines == 0)
                newText = "\r\n\r\n>\r\n\r\n";

            else if (numPreviousEmptyLines == 1)
                newText = "\r\n>\r\n\r\n";

            tbNote.Document.Insert(startPos, newText);
            tbNote.Select(startPos + newText.Length - 4, 0);
        }

        private void AddSelectionQuotes(TextEditor tbNote)
        {
            var numPreviousEmptyLines = GetNumberOfPreviousLineBreaks(tbNote);
            var numLines = tbNote.SelectedText.Count(c => c == '\n');
            var startPos = tbNote.SelectionStart;
            string newText = "";

            if (tbNote.SelectionStart == 0 || numPreviousEmptyLines >= 2)
                newText = ">" + tbNote.SelectedText.Replace("\r\n", "  \r\n>") + "  \r\n\r\n";

            else if (numPreviousEmptyLines == 0)
                newText = "\r\n\r\n>" + tbNote.SelectedText.Replace("\r\n", "  \r\n>") + "  \r\n\r\n";

            else if (numPreviousEmptyLines == 1)
                newText = "\r\n>" + tbNote.SelectedText.Replace("\r\n", "  \r\n>") + "  \r\n\r\n";

            tbNote.Document.Replace(startPos, tbNote.SelectionLength, newText);
            SelectText(tbNote, startPos + newText.Length, 0);
        }

        public bool CanCodeExec(TextEditor tbNote)
        {
            return true;
        }

        public void CodeExec(TextEditor tbNote)
        {
            int columnPos = tbNote.TextArea.Caret.Position.Column;

            bool multiLine = tbNote.SelectionLength > columnPos + 1;

            if (multiLine)
            {
                var start = GetPreviousLineBreakPosition(tbNote);
                tbNote.Select(start, (tbNote.SelectionStart - start) + tbNote.SelectionLength);
                FormatText(tbNote, "```\r\n", "\r\n```");
            }
            else
            {
                FormatText(tbNote, "`", "`");
            }
        }

        public bool CanBulletPointExec(TextEditor tbNote)
        {   
            return true;
        }

        public void BulletPointExec(TextEditor tbNote)
        {

        }

        public bool CanListExec(TextEditor tbNote)
        {
            return true;
        }

        public void ListExec(TextEditor tbNote)
        {

        }

        public bool CanImageExec(TextEditor tbNote)
        {
            return true;
        }

        public void ImageExec(TextEditor tbNote)
        {

        }

        public bool CanLinkExec(TextEditor tbNote)
        {
            return true;
        }

        public void LinkExec(TextEditor tbNote)
        {

        }

        public bool CanHorizontalLineExec(TextEditor tbNote)
        {
            return true;
        }

        public void HorizontalLineExec(TextEditor tbNote)
        {
            if (tbNote.SelectionLength == 0)
            {
                InsertHorizontalLine(tbNote, tbNote.SelectionStart);
            }
            else
            {
                var start = GetPreviousLineBreakPosition(tbNote);
                InsertHorizontalLine(tbNote, start);
            }
        }

        private void InsertHorizontalLine(TextEditor tbNote, int startPos)
        {
            if (startPos > 0 && tbNote.Document.Text[startPos - 1] != '\n')
                FormatText(tbNote, "\n\n----------\n", null);
            else if (startPos < tbNote.Document.Text.Length && tbNote.Document.Text[startPos + 1] != '\n')
                FormatText(tbNote, "\n----------\n\n", null);
            else
                FormatText(tbNote, "\n----------\n", null);
        }

        private void FormatText(TextEditor tbNote, string before, string after)
        {
            var selectStart = tbNote.SelectionStart;
            var selectLength = tbNote.SelectionLength;
            var selectEnd = selectStart + selectLength + before.Length;

            var newText = "";
            if (!String.IsNullOrWhiteSpace(before))
                newText += before;

            newText += tbNote.SelectedText;

            if (!String.IsNullOrWhiteSpace(after))
                newText += after;

            tbNote.Document.Replace(tbNote.SelectionStart, tbNote.SelectionLength, newText);
            SelectText(tbNote, selectStart + before.Length, selectLength);
        }

        private void SelectText(TextEditor tbNote, int start, int length)
        {
            tbNote.Focus();
            tbNote.Select(start, length);
        }

        private int GetPreviousLineBreakPosition(TextEditor tbNote)
        {
            var start = tbNote.SelectionStart;

            while (start > 0 && tbNote.Document.Text[start - 1] != '\n')
                start--;

            return start;
        }

        private int GetNumberOfPreviousLineBreaks(TextEditor tbNote)
        {
            var start = tbNote.SelectionStart - 1;
            int numLines = 0;

            while (start > 0)
            {
                if (tbNote.Document.Text[start] == '\n')
                    numLines++;

                else if (tbNote.Document.Text[start] == '\r')
                { }

                else
                    break;  // Any character not a line break is the end of the search

                start--;
            }

            return numLines;
        }
    }
}

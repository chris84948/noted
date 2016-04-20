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
            FormatText(tbNote, "__", "__");
        }

        public bool CanStrikethroughExec(TextEditor tbNote)
        {
            return true;
        }

        public void StrikethroughExec(TextEditor tbNote)
        {
            FormatText(tbNote, "~~", "~~");
        }

        public bool CanQuotesExec(TextEditor tbNote)
        {
            return true;
        }

        public void QuotesExec(TextEditor tbNote)
        {
            var start = GetPreviousLineBreak(tbNote);
            var highlightedLength = (tbNote.SelectionStart - start) + tbNote.SelectionLength;
            var selectedText = tbNote.Document.Text.Substring(start, highlightedLength);

            var numLines = selectedText.Count(c => c == '\n');
            var newText = selectedText.Replace("\n", "\n>");

            tbNote.Document.Replace(start, highlightedLength, newText);
            SelectText(tbNote, start, newText.Length);
        }

        public bool CanCodeExec(TextEditor tbNote)
        {
            return true;
        }

        public void CodeExec(TextEditor tbNote)
        {
            if (tbNote.Document.Text[tbNote.SelectionStart - 1] == '\n' &&
                tbNote.Document.Text[tbNote.SelectionStart + tbNote.SelectionLength + 1] == '\n')
            {
                var start = GetPreviousLineBreak(tbNote);
                tbNote.Select(start, (tbNote.SelectionStart - start) + tbNote.SelectionLength);
                FormatText(tbNote, "```\n", "\n```");
            }
            else
            {
                FormatText(tbNote, "``", "``");
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
                var start = GetPreviousLineBreak(tbNote);
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

        private int GetPreviousLineBreak(TextEditor tbNote)
        {
            var start = tbNote.SelectionStart;

            while (start > 0 && tbNote.Document.Text[start] != '\n')
                start--;

            return start;
        }
    }
}

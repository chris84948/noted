﻿using ICSharpCode.AvalonEdit;
using JustMVVM;
using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        public ICommand EnterCommand { get { return new RelayCommand<TextEditor>(EnterExec, CanEnterExec); } }

        public bool CanHeader1Exec(TextEditor tbNote)
        {
            return true;
        }

        public void Header1Exec(TextEditor tbNote)
        {
            FormatTextMultiLine(tbNote, "# ", "");
        }

        public bool CanHeader2Exec(TextEditor tbNote)
        {
            return true;
        }

        public void Header2Exec(TextEditor tbNote)
        {
            FormatTextMultiLine(tbNote, "## ", "");
        }

        public bool CanHeader3Exec(TextEditor tbNote)
        {
            return true;
        }

        public void Header3Exec(TextEditor tbNote)
        {
            FormatTextMultiLine(tbNote, "### ", "");
        }

        public bool CanHeader4Exec(TextEditor tbNote)
        {
            return true;
        }

        public void Header4Exec(TextEditor tbNote)
        {
            FormatTextMultiLine(tbNote, "#### ", "");
        }

        public bool CanHeader5Exec(TextEditor tbNote)
        {
            return true;
        }

        public void Header5Exec(TextEditor tbNote)
        {
            FormatTextMultiLine(tbNote, "##### ", "");
        }

        public bool CanHeader6Exec(TextEditor tbNote)
        {
            return true;
        }

        public void Header6Exec(TextEditor tbNote)
        {
            FormatTextMultiLine(tbNote, "###### ", "");
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
                FormatTextMultiLine(tbNote, ">", "");

            else
                FormatTextMultiLineQuotes(tbNote);
        }

        private void FormatTextMultiLineQuotes(TextEditor tbNote)
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

            if (columnPos > 0 && tbNote.SelectionLength > 0 && !tbNote.SelectedText.Contains('\n')) // Less than 1 line highlighted
                FormatText(tbNote, "`", "`");

            else // Use multiline code markdown
                FormatTextMultiLine(tbNote, "```\r\n", "\r\n```");
        }

        public bool CanBulletPointExec(TextEditor tbNote)
        {   
            return true;
        }
        
        public void BulletPointExec(TextEditor tbNote)
        {
            FormatList(tbNote, bulletPoint: true);
        }

        public bool CanListExec(TextEditor tbNote)
        {
            return true;
        }

        public void ListExec(TextEditor tbNote)
        {
            FormatList(tbNote, bulletPoint: false);
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

        public bool CanEnterExec(TextEditor tbNote)
        {
            return true;
        }

        public void EnterExec(TextEditor tbNote)
        {
            var line = tbNote.Document.GetLineByNumber(tbNote.TextArea.Caret.Line - 1);
            var lineText = tbNote.Document.GetText(line.Offset, line.Length);

            if (Regex.IsMatch(lineText, @"^\-\ "))  // Bullet list
            {
                if (lineText.Length == 2)   // Empty line on bullet list, end list
                    tbNote.Document.Replace(tbNote.SelectionStart - 4, 4, "\r\n");
                else
                    tbNote.Document.Insert(tbNote.Text.Length, "- ");
            }
            else if (Regex.IsMatch(lineText, @"^\d+\.\ "))  // Numbered List
            {
                var match = Regex.Match(lineText, @"^(\d+)\.");
                int nextNum = Convert.ToInt32(match.Groups[1].Value) + 1;

                if (lineText.Length == nextNum.ToString().Length + 2)   // Empty line on numbered list, end list
                    tbNote.Document.Replace(tbNote.SelectionStart - 5, 5, "\r\n");
                else
                    tbNote.Document.Insert(tbNote.Text.Length, nextNum.ToString() + ". ");
            }
        }

        private void InsertHorizontalLine(TextEditor tbNote, int startPos)
        {
            if (startPos > 0 && tbNote.Text[startPos - 1] != '\n')
                FormatText(tbNote, "\r\n\r\n----------\r\n\r\n", "");
            else if (startPos < tbNote.Text.Length && tbNote.Text[startPos + 1] != '\n')
                FormatText(tbNote, "\r\n----------\r\n\r\n", "");
            else
                FormatText(tbNote, "\r\n----------\r\n\r\n", "");
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

        private void FormatTextMultiLine(TextEditor tbNote, string before, string after)
        {
            var numPreviousEmptyLines = GetNumberOfPreviousLineBreaks(tbNote);
            var startPos = tbNote.SelectionStart;
            var newText = "";

            if (tbNote.SelectionStart == 0 || numPreviousEmptyLines >= 2)
                newText = before + tbNote.SelectedText + after + "\r\n\r\n";

            else if (numPreviousEmptyLines == 0)
                newText = "\r\n\r\n" + before + tbNote.SelectedText + after + "\r\n\r\n";

            else if (numPreviousEmptyLines == 1)
                newText = "\r\n" + before + tbNote.SelectedText + after + "\r\n\r\n";

            tbNote.Document.Replace(startPos, tbNote.SelectionLength, newText);

            if (tbNote.SelectionLength == 0)
                SelectText(tbNote, startPos + newText.Length - 4 - after.Length, 0);
            else
                SelectText(tbNote, startPos + newText.Length, 0);
        }

        private void FormatList(TextEditor tbNote, bool bulletPoint)
        {
            var numPreviousEmptyLines = GetNumberOfPreviousLineBreaks(tbNote);
            var startPos = tbNote.SelectionStart;
            var textAsList = bulletPoint ? GetBulletList(tbNote.SelectedText) : GetNumberList(tbNote.SelectedText);
            var newText = "";

            if (tbNote.SelectionStart == 0 || numPreviousEmptyLines >= 2)
                newText = textAsList + "\r\n\r\n";

            else if (numPreviousEmptyLines == 0)
                newText = "\r\n\r\n" + textAsList + "\r\n\r\n";

            else if (numPreviousEmptyLines == 1)
                newText = "\r\n" + textAsList + "\r\n\r\n";

            tbNote.Document.Replace(startPos, tbNote.SelectionLength, newText);
            SelectText(tbNote, startPos + newText.Length - 4, 0);
        }

        private string GetBulletList(string selectedText)
        {
            StringBuilder bulletLineText = new StringBuilder();
            bulletLineText.Append("- " + selectedText.Replace("\r\n", "\r\n- "));

            if (selectedText.Length > 0 && !bulletLineText.ToString().EndsWith("\r\n- "))
                bulletLineText.Append("\r\n- ");

            return bulletLineText.ToString();
        }

        private string GetNumberList(string selectedText)
        {
            StringBuilder numberText = new StringBuilder();
            int listNum = 1;

            string[] splitText = selectedText.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            numberText.Append(listNum++.ToString() + ". " + (splitText.Length > 0 ? splitText[0] : ""));

            for (int i = 1; i < splitText.Length; i++)
                numberText.Append("\r\n" + listNum++.ToString() + ". " + splitText[i]);

            if (splitText.Length > 1)
                numberText.Remove(numberText.Length - 3, 2);

            if (selectedText.Length > 0 && !Regex.IsMatch(numberText.ToString(), @"\n\d\\.\ \n?$"))
                numberText.Append("\r\n" + listNum.ToString() + ". ");

            return numberText.ToString();
        }

        private void SelectText(TextEditor tbNote, int start, int length)
        {
            tbNote.Focus();
            tbNote.Select(start, length);
        }

        private int GetPreviousLineBreakPosition(TextEditor tbNote)
        {
            var start = tbNote.SelectionStart;

            while (start > 0 && tbNote.Text[start - 1] != '\n')
                start--;

            return start;
        }

        private int GetNumberOfPreviousLineBreaks(TextEditor tbNote)
        {
            var start = tbNote.SelectionStart - 1;
            int numLines = 0;

            while (start > 0)
            {
                if (tbNote.Text[start] == '\n')
                    numLines++;

                else if (tbNote.Text[start] == '\r')
                { }

                else
                    break;  // Any character not a line break is the end of the search

                start--;
            }

            return numLines;
        }
    }
}

using ICSharpCode.AvalonEdit;
using JustMVVM;
using NotedUI.Models;
using NotedUI.Utilities;
using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace NotedUI.UI.ViewModels
{
    public class FormatCommands : MVVMBase
    {
        public ICommand Header1Command { get { return new RelayCommand<TextEditor>(Header1Exec); } }
        public ICommand Header2Command { get { return new RelayCommand<TextEditor>(Header2Exec); } }
        public ICommand Header3Command { get { return new RelayCommand<TextEditor>(Header3Exec); } }
        public ICommand Header4Command { get { return new RelayCommand<TextEditor>(Header4Exec); } }
        public ICommand Header5Command { get { return new RelayCommand<TextEditor>(Header5Exec); } }
        public ICommand Header6Command { get { return new RelayCommand<TextEditor>(Header6Exec); } }
        public ICommand BoldCommand { get { return new RelayCommand<TextEditor>(BoldExec); } }
        public ICommand ItalicCommand { get { return new RelayCommand<TextEditor>(ItalicExec); } }
        public ICommand UnderlineCommand { get { return new RelayCommand<TextEditor>(UnderlineExec); } }
        public ICommand StrikethroughCommand { get { return new RelayCommand<TextEditor>(StrikethroughExec); } }
        public ICommand QuotesCommand { get { return new RelayCommand<TextEditor>(QuotesExec); } }
        public ICommand CodeCommand { get { return new RelayCommand<TextEditor>(CodeExec); } }
        public ICommand BulletPointCommand { get { return new RelayCommand<TextEditor>(BulletPointExec); } }
        public ICommand ListCommand { get { return new RelayCommand<TextEditor>(ListExec); } }
        public ICommand ImageCommand { get { return new RelayCommand<TextEditor>(ImageExec); } }
        public ICommand LinkCommand { get { return new RelayCommand<TextEditor>(LinkExec); } }
        public ICommand HorizontalLineCommand { get { return new RelayCommand<TextEditor>(HorizontalLineExec); } }
        public ICommand LineBreakCommand { get { return new RelayCommand<TextEditor>(LineBreakExec); } }

        public ICommand EnterCommand { get { return new RelayCommand<TextEditor>(EnterExec); } }

        public ICommand ShowFindDialogCommand { get { return new RelayCommand(ShowFindDialogExec); } }
        public ICommand ShowReplaceDialogCommand { get { return new RelayCommand(ShowReplaceDialogExec); } }
        public ICommand HideFindDialogCommand { get { return new RelayCommand(HideFindDialogExec); } }

        public ICommand CopyMarkupToClipboard { get { return new RelayCommand<TextEditor>(CopyMarkupToClipboardExec); } }

        private bool _showFindDialog;
        public bool ShowFindDialog
        {
            get { return _showFindDialog; }
            set
            {
                _showFindDialog = value;
                OnPropertyChanged();
            }
        }

        private bool _showReplaceDialog;
        public bool ShowReplaceDialog
        {
            get { return _showReplaceDialog; }
            set
            {
                _showReplaceDialog = value;
                OnPropertyChanged();
            }
        }

        private HomeViewModel _homeVM;

        public FormatCommands(HomeViewModel homeVM)
        {
            _homeVM = homeVM;
        }

        public void Header1Exec(TextEditor tbNote)
        {
            FormatTextMultiLine(tbNote, "# ", "");
        }

        public void Header2Exec(TextEditor tbNote)
        {
            FormatTextMultiLine(tbNote, "## ", "");
        }

        public void Header3Exec(TextEditor tbNote)
        {
            FormatTextMultiLine(tbNote, "### ", "");
        }

        public void Header4Exec(TextEditor tbNote)
        {
            FormatTextMultiLine(tbNote, "#### ", "");
        }

        public void Header5Exec(TextEditor tbNote)
        {
            FormatTextMultiLine(tbNote, "##### ", "");
        }

        public void Header6Exec(TextEditor tbNote)
        {
            FormatTextMultiLine(tbNote, "###### ", "");
        }

        public void BoldExec(TextEditor tbNote)
        {
            FormatText(tbNote, "**", "**");
        }

        public void ItalicExec(TextEditor tbNote)
        {
            FormatText(tbNote, "*", "*");
        }

        public void UnderlineExec(TextEditor tbNote)
        {
            FormatText(tbNote, "<u>", "</u>");
        }

        public void StrikethroughExec(TextEditor tbNote)
        {
            FormatText(tbNote, "<s>", "</s>");
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

        public void CodeExec(TextEditor tbNote)
        {
            int columnPos = tbNote.TextArea.Caret.Position.Column;

            if (columnPos > 0 && tbNote.SelectionLength > 0 && !tbNote.SelectedText.Contains('\n')) // Less than 1 line highlighted
                FormatText(tbNote, "`", "`");

            else // Use multiline code markdown
                FormatTextMultiLine(tbNote, "```\r\n", "\r\n```");
        }

        public void BulletPointExec(TextEditor tbNote)
        {
            FormatList(tbNote, bulletPoint: true);
        }

        public void ListExec(TextEditor tbNote)
        {
            FormatList(tbNote, bulletPoint: false);
        }

        public void ImageExec(TextEditor tbNote)
        {

        }

        public void LinkExec(TextEditor tbNote)
        {
            var dialog = new LinkDialogViewModel(tbNote.SelectedText);

            dialog.DialogClosed += async () =>
            {
                await Task.Delay(300);
                _homeVM.FixAirspace = false;

                if (dialog.Result == System.Windows.Forms.DialogResult.Cancel)
                    return;
                
                var start = tbNote.SelectionStart;
                var link = $"[{ dialog.Description }]({ dialog.Link })";
                tbNote.Document.Replace(start, tbNote.SelectionLength, link);
                tbNote.Focus();
                tbNote.Select(start + link.Length, 0);
            };

            _homeVM.InvokeShowDialog(dialog);
        }

        public void HorizontalLineExec(TextEditor tbNote)
        {
            if (tbNote.SelectionLength == 0)
            {
                InsertContentInLineBreaks(tbNote, tbNote.SelectionStart, "----------");
            }
            else
            {
                var start = GetPreviousLineBreakPosition(tbNote);
                InsertContentInLineBreaks(tbNote, start, "----------");
            }
        }

        public void EnterExec(TextEditor tbNote)
        {
            var line = tbNote.Document.GetLineByNumber(tbNote.TextArea.Caret.Line - 1);
            var lineText = tbNote.Document.GetText(line.Offset, line.Length);

            var bulletMatch = Regex.Match(lineText, @"^((\s+)?\-\ )");

            if (bulletMatch.Success)  // Bullet list
            {
                string bulletMatchText = bulletMatch.Groups[1].Value;

                if (lineText.Length == bulletMatch.Length)   // Empty line on bullet list, end list
                    tbNote.Document.Replace(tbNote.SelectionStart - bulletMatchText.Length - 2, bulletMatchText.Length + 2, "\r\n");
                else
                    tbNote.Document.Insert(tbNote.SelectionStart, "- ");
            }
            else if (Regex.IsMatch(lineText, @"^\d+\.\ "))  // Numbered List
            {
                var match = Regex.Match(lineText, @"^(\d+)\.");
                int currentNum = Convert.ToInt32(match.Groups[1].Value);
                int currentNumStringLength = currentNum.ToString().Length;

                if (lineText.Length == currentNumStringLength + 2)   // Empty line on numbered list, end list
                    tbNote.Document.Replace(tbNote.SelectionStart - (4 + currentNumStringLength), 4 + currentNumStringLength, "\r\n");
                else
                    tbNote.Document.Insert(tbNote.SelectionStart, (currentNum + 1).ToString() + ". ");
            }
        }

        public void LineBreakExec(TextEditor tbNote)
        {
            if (tbNote.SelectionLength == 0)
            {
                InsertContentInLineBreaks(tbNote, tbNote.SelectionStart, "<br></br>");
            }
            else
            {
                var start = GetPreviousLineBreakPosition(tbNote);
                InsertContentInLineBreaks(tbNote, start, "<br></br>");
            }
        }

        private void InsertContentInLineBreaks(TextEditor tbNote, int startPos, string content)
        {
            if (startPos > 0 && tbNote.Text[startPos - 1] != '\n')
                FormatText(tbNote, "\r\n\r\n" + content + "\r\n\r\n", "");
            else if (startPos < tbNote.Text.Length && tbNote.Text[startPos + 1] != '\n')
                FormatText(tbNote, "\r\n" + content + "\r\n\r\n", "");
            else
                FormatText(tbNote, "\r\n" + content + "\r\n\r\n", "");
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

        private void ShowFindDialogExec()
        {
            ShowFindDialog = true;
        }

        private void ShowReplaceDialogExec()
        {
            ShowFindDialog = true;
            ShowReplaceDialog = true;
        }

        private void HideFindDialogExec()
        {
            ShowFindDialog = false;
            ShowReplaceDialog = false;
        }
        
        private void CopyMarkupToClipboardExec(TextEditor editor)
        {
            if (editor.SelectionLength == 0)
                Clipboard.SetText(MarkdownParser.Parse(editor.Text));
            else
                Clipboard.SetText(MarkdownParser.Parse(editor.SelectedText));
        }
    }
}

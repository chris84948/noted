using ICSharpCode.AvalonEdit;
using JustMVVM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NotedUI.UI.Dialogs
{
    /// <summary>
    /// Interaction logic for FindReplaceDialog.xaml
    /// </summary>
    public partial class FindReplaceDialog : UserControl
    {
        public ICommand SearchNextCommand { get { return new RelayCommand(SearchNextExec, () => Editor?.Text != null); } }
        public ICommand SearchPreviousCommand { get { return new RelayCommand(SearchPreviousExec, () => Editor?.Text != null); } }
        public ICommand ReplaceCommand { get { return new RelayCommand(ReplaceExec, () => Editor?.Text != null); } }
        public ICommand ReplaceAllCommand { get { return new RelayCommand(ReplaceAllExec, () => Editor?.Text != null); } }
        public ICommand MatchCaseToggleCommand { get { return new RelayCommand(MatchCaseToggleExec, () => Editor?.Text != null); } }
        public ICommand MatchWordToggleCommand { get { return new RelayCommand(MatchWordToggleExec, () => Editor?.Text != null); } }
        public ICommand RegexToggleCommand { get { return new RelayCommand(RegexToggleExec, () => Editor?.Text != null); } }
        public ICommand HideSearchCommand { get { return new RelayCommand(HideSearchExec, () => Editor?.Text != null); } }
        public ICommand ShowFindCommand { get { return new RelayCommand(ShowFindExec, () => Editor?.Text != null); } }
        public ICommand ShowReplaceCommand { get { return new RelayCommand(ShowReplaceExec, () => Editor?.Text != null); } }
        public ICommand FocusDialogCommand { get { return new RelayCommand(DialogShownExec); } }

        public ICommand DialogShownCommand { get { return new RelayCommand(DialogShownExec); } }

        public static readonly DependencyProperty EditorProperty =
            DependencyProperty.Register("Editor",
                                        typeof(TextEditor),
                                        typeof(FindReplaceDialog),
                                        new FrameworkPropertyMetadata(null));

        public TextEditor Editor
        {
            get { return (TextEditor)GetValue(EditorProperty); }
            set { SetValue(EditorProperty, value); }
        }

        public static readonly DependencyProperty ShowReplaceProperty =
            DependencyProperty.Register("ShowReplace",
                                        typeof(bool),
                                        typeof(FindReplaceDialog),
                                        new FrameworkPropertyMetadata(false));

        public bool ShowReplace
        {
            get { return (bool)GetValue(ShowReplaceProperty); }
            set { SetValue(ShowReplaceProperty, value); }
        }

        public static readonly RoutedEvent FindDialogHiddenEvent =
            EventManager.RegisterRoutedEvent("FindDialogHidden",
                                             RoutingStrategy.Bubble,
                                             typeof(RoutedEventHandler),
                                             typeof(FindReplaceDialog));

        public event RoutedEventHandler FindDialogHidden
        {
            add { AddHandler(FindDialogHiddenEvent, value); }
            remove { RemoveHandler(FindDialogHiddenEvent, value); }
        }

        public static readonly RoutedEvent FindShownEvent =
            EventManager.RegisterRoutedEvent("FindShown",
                                             RoutingStrategy.Bubble,
                                             typeof(RoutedEventHandler),
                                             typeof(FindReplaceDialog));

        public event RoutedEventHandler FindShown
        {
            add { AddHandler(FindShownEvent, value); }
            remove { RemoveHandler(FindShownEvent, value); }
        }
        
        public static readonly RoutedEvent ReplaceShownEvent =
            EventManager.RegisterRoutedEvent("ReplaceShown",
                                             RoutingStrategy.Bubble,
                                             typeof(RoutedEventHandler),
                                             typeof(FindReplaceDialog));

        public event RoutedEventHandler ReplaceShown
        {
            add { AddHandler(ReplaceShownEvent, value); }
            remove { RemoveHandler(ReplaceShownEvent, value); }
        }
        
        
        public FindReplaceDialog()
        {
            InitializeComponent();
        }

        private void tbReplace_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void tbFind_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                SearchNextExec();
        }

        private void SearchNextExec()
        {
            if (buttonRegex.IsChecked == true || buttonMatchWord.IsChecked == true)
            {
                if (buttonMatchWord.IsChecked == true)
                    SearchPositionNextRegex($@"\b{ tbFind.Text }\b");

                else
                    SearchPositionNextRegex(tbFind.Text);
            }
            else
            {
                 SearchPositionNext();
            }
        }

        private void SearchPreviousExec()
        {
            if (buttonRegex.IsChecked == true || buttonMatchWord.IsChecked == true)
            {
                if (buttonMatchWord.IsChecked == true)
                    SearchPositionPreviousRegex($@"\b{ tbFind.Text }\b");

                else
                    SearchPositionPreviousRegex(tbFind.Text);
            }
            else
            {
                SearchPositionPrevious();
            }
        }

        private void ReplaceExec()
        {
            Editor.Document.Replace(Editor.SelectionStart, Editor.SelectionLength, tbReplace.Text);
        }

        private void ReplaceAllExec()
        {

            Editor.Document.Replace(Editor.SelectionStart, Editor.SelectionLength, tbReplace.Text);
        }

        private void MatchCaseToggleExec()
        {
            buttonMatchCase.IsChecked = !buttonMatchCase.IsChecked;
        }

        private void MatchWordToggleExec()
        {
            buttonMatchWord.IsChecked = !buttonMatchWord.IsChecked;
        }

        private void RegexToggleExec()
        {
            buttonRegex.IsChecked = !buttonRegex.IsChecked;
        }

        private void HideSearchExec()
        {
            RoutedEventArgs routedArgs = new RoutedEventArgs(FindDialogHiddenEvent);
            RaiseEvent(routedArgs);
        }

        private void ShowFindExec()
        {
            RoutedEventArgs routedArgs = new RoutedEventArgs(FindShownEvent);
            RaiseEvent(routedArgs);
        }

        private void ShowReplaceExec()
        {
            RoutedEventArgs routedArgs = new RoutedEventArgs(ReplaceShownEvent);
            RaiseEvent(routedArgs);
        }

        private void DialogShownExec()
        {
            tbFind.Focus();
            tbFind.Text = Editor.SelectedText;
            tbFind.SelectAll();
        }

        private string regexMem = "";
        private MatchCollection allMatches;
        private int matchIndex;

        private void SearchPositionNextRegex(string regex)
        {
            if (allMatches == null || regexMem != regex)
            {
                allMatches = GetAllRegexMatches(regex);
                regexMem = regex;
            }

            matchIndex = GetNextMatchIndex(allMatches);

            if (matchIndex == -1)
                SelectTextForMatch(-1, 0);
            else
                SelectTextForMatch(allMatches[matchIndex].Index, allMatches[matchIndex].Value.Length);
        }

        private void SearchPositionPreviousRegex(string regex)
        {
            if (allMatches == null || regexMem != regex)
            {
                allMatches = GetAllRegexMatches(regex);
                regexMem = regex;
            }

            matchIndex = GetPreviousMatchIndex(allMatches);

            if (matchIndex == -1)
                SelectTextForMatch(-1, 0);
            else
                SelectTextForMatch(allMatches[matchIndex].Index, allMatches[matchIndex].Value.Length);
        }

        private MatchCollection GetAllRegexMatches(string regex)
        {
            return Regex.Matches(Editor.Text, 
                                 regex,
                                 buttonMatchCase.IsChecked == true ? RegexOptions.None : RegexOptions.IgnoreCase);
        }

        private int GetNextMatchIndex(MatchCollection matches)
        {
            if (matches.Count == 0)
                return -1;

            for (int i = 0; i < matches.Count; i++)
            {
                if (matches[i].Index > Editor.SelectionStart)
                    return i;
            }

            // Grab the first match again, basically wrap around to the first one again
            return 0;
        }

        private int GetPreviousMatchIndex(MatchCollection matches)
        {
            if (matches.Count == 0)
                return -1;

            for (int i = matches.Count - 1; i >= 0; i--)
            {
                if (matches[i].Index < Editor.SelectionStart)
                    return i;
            }

            // Grab the last match again, basically wrap around to the last one again
            return matches.Count - 1;
        }

        private void SearchPositionNext()
        {
            int foundPosition = Editor.Text.IndexOf(tbFind.Text, 
                                                Editor.SelectionStart + Editor.SelectionLength,
                                                buttonMatchCase.IsChecked == true ? StringComparison.CurrentCulture : StringComparison.CurrentCultureIgnoreCase);

            if (foundPosition == -1)
                // Start again from the start
                foundPosition = Editor.Text.IndexOf(tbFind.Text,
                                                    buttonMatchCase.IsChecked == true ? StringComparison.CurrentCulture : StringComparison.CurrentCultureIgnoreCase);

            SelectTextForMatch(foundPosition, tbFind.Text.Length);
        }

        private void SearchPositionPrevious()
        {
            int foundPosition = Editor.Text.LastIndexOf(tbFind.Text,
                                                        Editor.SelectionStart,
                                                        buttonMatchCase.IsChecked == true ? StringComparison.CurrentCulture : StringComparison.CurrentCultureIgnoreCase);

            if (foundPosition == -1)
                // Start again from the end
                foundPosition = Editor.Text.LastIndexOf(tbFind.Text,
                                                        Editor.Text.Length - 1,
                                                        buttonMatchCase.IsChecked == true ? StringComparison.CurrentCulture : StringComparison.CurrentCultureIgnoreCase);

            SelectTextForMatch(foundPosition, tbFind.Text.Length);
        }

        private void SelectTextForMatch(int position, int length)
        {
            if (position != -1)
                Editor.Select(position, length);

            else // Unhighlight text when no matches are found
                Editor.Select(Editor.SelectionStart, 0);
        }
    }
}

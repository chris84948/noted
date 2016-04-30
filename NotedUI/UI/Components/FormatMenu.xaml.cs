using ICSharpCode.AvalonEdit;
using JustMVVM;
using NotedUI.Models;
using NotedUI.UI.ViewModels;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System;

namespace NotedUI.UI.Components
{
    /// <summary>
    /// Interaction logic for FormatMenu.xaml
    /// </summary>
    public partial class FormatMenu : UserControl
    {
        public ICommand ShowFindCommand { get { return new RelayCommand(ShowFindExec); } }
        public ICommand ShowReplaceCommand { get { return new RelayCommand(ShowReplaceExec); } }
        public ICommand HideFindDialogCommand { get { return new RelayCommand(HideFindDialogExec); } }

        public static readonly DependencyProperty TextBoxProperty =
            DependencyProperty.Register("TextBox", 
                                        typeof(TextEditor), 
                                        typeof(FormatMenu),
                                        new FrameworkPropertyMetadata(null));

        public TextEditor TextBox
        {
            get { return (TextEditor)GetValue(TextBoxProperty); }
            set { SetValue(TextBoxProperty, value); }
        }

        public static readonly DependencyProperty HomeViewModelProperty =
            DependencyProperty.Register("HomeViewModel",
                                        typeof(HomeViewModel),
                                        typeof(FormatMenu),
                                        new FrameworkPropertyMetadata(null));

        public HomeViewModel HomeViewModel
        {
            get { return (HomeViewModel)GetValue(HomeViewModelProperty); }
            set { SetValue(HomeViewModelProperty, value); }
        }

        public static readonly DependencyProperty ShowFindDialogProperty =
            DependencyProperty.Register("ShowFindDialog",
                                        typeof(bool),
                                        typeof(FormatMenu),
                                        new FrameworkPropertyMetadata(false));

        public bool ShowFindDialog
        {
            get { return (bool)GetValue(ShowFindDialogProperty); }
            set { SetValue(ShowFindDialogProperty, value); }
        }

        public static readonly DependencyProperty ShowReplaceProperty =
            DependencyProperty.Register("ShowReplace",
                                        typeof(bool),
                                        typeof(FormatMenu),
                                        new FrameworkPropertyMetadata(false));

        public bool ShowReplace
        {
            get { return (bool)GetValue(ShowReplaceProperty); }
            set { SetValue(ShowReplaceProperty, value); }
        }

        public static readonly RoutedEvent FocusFindDialogEvent =
            EventManager.RegisterRoutedEvent("FocusFindDialog",
                                             RoutingStrategy.Bubble,
                                             typeof(RoutedEventHandler),
                                             typeof(FormatMenu));

        public event RoutedEventHandler FocusFindDialog
        {
            add { AddHandler(FocusFindDialogEvent, value); }
            remove { RemoveHandler(FocusFindDialogEvent, value); }
        }
        

        public static readonly DependencyProperty Header1CommandProperty =
            DependencyProperty.Register("Header1Command",
                                        typeof(ICommand),
                                        typeof(FormatMenu),
                                        new PropertyMetadata((ICommand)null));

        [TypeConverter(typeof(CommandConverter))]
        public ICommand Header1Command
        {
            get { return (ICommand)GetValue(Header1CommandProperty); }
            set { SetValue(Header1CommandProperty, value); }
        }

        public static readonly DependencyProperty Header2CommandProperty =
            DependencyProperty.Register("Header2Command",
                                        typeof(ICommand),
                                        typeof(FormatMenu),
                                        new PropertyMetadata((ICommand)null));

        [TypeConverter(typeof(CommandConverter))]
        public ICommand Header2Command
        {
            get { return (ICommand)GetValue(Header2CommandProperty); }
            set { SetValue(Header2CommandProperty, value); }
        }

        public static readonly DependencyProperty Header3CommandProperty =
            DependencyProperty.Register("Header3Command",
                                        typeof(ICommand),
                                        typeof(FormatMenu),
                                        new PropertyMetadata((ICommand)null));

        [TypeConverter(typeof(CommandConverter))]
        public ICommand Header3Command
        {
            get { return (ICommand)GetValue(Header3CommandProperty); }
            set { SetValue(Header3CommandProperty, value); }
        }

        public static readonly DependencyProperty Header4CommandProperty =
            DependencyProperty.Register("Header4Command",
                                        typeof(ICommand),
                                        typeof(FormatMenu),
                                        new PropertyMetadata((ICommand)null));

        [TypeConverter(typeof(CommandConverter))]
        public ICommand Header4Command
        {
            get { return (ICommand)GetValue(Header4CommandProperty); }
            set { SetValue(Header4CommandProperty, value); }
        }

        public static readonly DependencyProperty Header5CommandProperty =
            DependencyProperty.Register("Header5Command",
                                        typeof(ICommand),
                                        typeof(FormatMenu),
                                        new PropertyMetadata((ICommand)null));

        [TypeConverter(typeof(CommandConverter))]
        public ICommand Header5Command
        {
            get { return (ICommand)GetValue(Header5CommandProperty); }
            set { SetValue(Header5CommandProperty, value); }
        }

        public static readonly DependencyProperty Header6CommandProperty =
            DependencyProperty.Register("Header6Command",
                                        typeof(ICommand),
                                        typeof(FormatMenu),
                                        new PropertyMetadata((ICommand)null));

        [TypeConverter(typeof(CommandConverter))]
        public ICommand Header6Command
        {
            get { return (ICommand)GetValue(Header6CommandProperty); }
            set { SetValue(Header6CommandProperty, value); }
        }

        public static readonly DependencyProperty BoldCommandProperty =
            DependencyProperty.Register("BoldCommand",
                                        typeof(ICommand),
                                        typeof(FormatMenu),
                                        new PropertyMetadata((ICommand)null));

        [TypeConverter(typeof(CommandConverter))]
        public ICommand BoldCommand
        {
            get { return (ICommand)GetValue(BoldCommandProperty); }
            set { SetValue(BoldCommandProperty, value); }
        }

        public static readonly DependencyProperty ItalicCommandProperty =
            DependencyProperty.Register("ItalicCommand",
                                        typeof(ICommand),
                                        typeof(FormatMenu),
                                        new PropertyMetadata((ICommand)null));

        [TypeConverter(typeof(CommandConverter))]
        public ICommand ItalicCommand
        {
            get { return (ICommand)GetValue(ItalicCommandProperty); }
            set { SetValue(ItalicCommandProperty, value); }
        }

        public static readonly DependencyProperty StrikethroughCommandProperty =
            DependencyProperty.Register("StrikethroughCommand",
                                        typeof(ICommand),
                                        typeof(FormatMenu),
                                        new PropertyMetadata((ICommand)null));

        [TypeConverter(typeof(CommandConverter))]
        public ICommand UnderlineCommand
        {
            get { return (ICommand)GetValue(UnderlineCommandProperty); }
            set { SetValue(UnderlineCommandProperty, value); }
        }

        public static readonly DependencyProperty UnderlineCommandProperty =
            DependencyProperty.Register("UnderlineCommand",
                                        typeof(ICommand),
                                        typeof(FormatMenu),
                                        new PropertyMetadata((ICommand)null));

        [TypeConverter(typeof(CommandConverter))]
        public ICommand StrikethroughCommand
        {
            get { return (ICommand)GetValue(StrikethroughCommandProperty); }
            set { SetValue(StrikethroughCommandProperty, value); }
        }

        public static readonly DependencyProperty QuotesCommandProperty =
            DependencyProperty.Register("QuotesCommand",
                                        typeof(ICommand),
                                        typeof(FormatMenu),
                                        new PropertyMetadata((ICommand)null));

        [TypeConverter(typeof(CommandConverter))]
        public ICommand QuotesCommand
        {
            get { return (ICommand)GetValue(QuotesCommandProperty); }
            set { SetValue(QuotesCommandProperty, value); }
        }

        public static readonly DependencyProperty CodeCommandProperty =
            DependencyProperty.Register("CodeCommand",
                                        typeof(ICommand),
                                        typeof(FormatMenu),
                                        new PropertyMetadata((ICommand)null));

        [TypeConverter(typeof(CommandConverter))]
        public ICommand CodeCommand
        {
            get { return (ICommand)GetValue(CodeCommandProperty); }
            set { SetValue(CodeCommandProperty, value); }
        }

        public static readonly DependencyProperty BulletPointCommandProperty =
            DependencyProperty.Register("BulletPointCommand",
                                        typeof(ICommand),
                                        typeof(FormatMenu),
                                        new PropertyMetadata((ICommand)null));

        [TypeConverter(typeof(CommandConverter))]
        public ICommand BulletPointCommand
        {
            get { return (ICommand)GetValue(BulletPointCommandProperty); }
            set { SetValue(BulletPointCommandProperty, value); }
        }

        public static readonly DependencyProperty ListCommandProperty =
            DependencyProperty.Register("ListCommand",
                                        typeof(ICommand),
                                        typeof(FormatMenu),
                                        new PropertyMetadata((ICommand)null));

        [TypeConverter(typeof(CommandConverter))]
        public ICommand ListCommand
        {
            get { return (ICommand)GetValue(ListCommandProperty); }
            set { SetValue(ListCommandProperty, value); }
        }

        public static readonly DependencyProperty ImageCommandProperty =
            DependencyProperty.Register("ImageCommand",
                                        typeof(ICommand),
                                        typeof(FormatMenu),
                                        new PropertyMetadata((ICommand)null));

        [TypeConverter(typeof(CommandConverter))]
        public ICommand ImageCommand
        {
            get { return (ICommand)GetValue(ImageCommandProperty); }
            set { SetValue(ImageCommandProperty, value); }
        }

        public static readonly DependencyProperty LinkCommandProperty =
            DependencyProperty.Register("LinkCommand",
                                        typeof(ICommand),
                                        typeof(FormatMenu),
                                        new PropertyMetadata((ICommand)null));

        [TypeConverter(typeof(CommandConverter))]
        public ICommand LinkCommand
        {
            get { return (ICommand)GetValue(LinkCommandProperty); }
            set { SetValue(LinkCommandProperty, value); }
        }

        public static readonly DependencyProperty HorizontalLineCommandProperty =
            DependencyProperty.Register("HorizontalLineCommand",
                                        typeof(ICommand),
                                        typeof(FormatMenu),
                                        new PropertyMetadata((ICommand)null));

        [TypeConverter(typeof(CommandConverter))]
        public ICommand HorizontalLineCommand
        {
            get { return (ICommand)GetValue(HorizontalLineCommandProperty); }
            set { SetValue(HorizontalLineCommandProperty, value); }
        }

        public static readonly DependencyProperty LineBreakCommandProperty =
            DependencyProperty.Register("LineBreakCommand",
                                        typeof(ICommand),
                                        typeof(FormatMenu),
                                        new PropertyMetadata((ICommand)null));

        [TypeConverter(typeof(CommandConverter))]
        public ICommand LineBreakCommand
        {
            get { return (ICommand)GetValue(LineBreakCommandProperty); }
            set { SetValue(LineBreakCommandProperty, value); }
        }
        
        //private static RoutedCommand _showFindCommand = new RoutedCommand();
        //public static RoutedCommand ShowFindCommand
        //{
        //    get { return _showFindCommand; }
        //}

        public FormatMenu()
        {
            InitializeComponent();
        }

        private void Header1_Click(object sender, RoutedEventArgs e)
        {
            if (Header1Command?.CanExecute(TextBox) == true)
                Header1Command?.Execute(TextBox);
        }

        private void Header2_Click(object sender, RoutedEventArgs e)
        {
            if (Header2Command?.CanExecute(TextBox) == true)
                Header2Command?.Execute(TextBox);
        }

        private void Header3_Click(object sender, RoutedEventArgs e)
        {
            if (Header3Command?.CanExecute(TextBox) == true)
                Header3Command?.Execute(TextBox);
        }

        private void Header4_Click(object sender, RoutedEventArgs e)
        {
            if (Header4Command?.CanExecute(TextBox) == true)
                Header4Command?.Execute(TextBox);
        }

        private void Header5_Click(object sender, RoutedEventArgs e)
        {
            if (Header5Command?.CanExecute(TextBox) == true)
                Header5Command?.Execute(TextBox);
        }

        private void Header6_Click(object sender, RoutedEventArgs e)
        {
            if (Header6Command?.CanExecute(TextBox) == true)
                Header6Command?.Execute(TextBox);
        }

        private void Bold_Click(object sender, RoutedEventArgs e)
        {
            if (BoldCommand?.CanExecute(TextBox) == true)
                BoldCommand?.Execute(TextBox);
        }

        private void Italic_Click(object sender, RoutedEventArgs e)
        {
            if (ItalicCommand?.CanExecute(TextBox) == true)
                ItalicCommand?.Execute(TextBox);
        }

        private void Underline_Click(object sender, RoutedEventArgs e)
        {
            if (UnderlineCommand?.CanExecute(TextBox) == true)
                UnderlineCommand?.Execute(TextBox);
        }

        private void Strikethrough_Click(object sender, RoutedEventArgs e)
        {
            if (StrikethroughCommand?.CanExecute(TextBox) == true)
                StrikethroughCommand?.Execute(TextBox);
        }

        private void Quote_Click(object sender, RoutedEventArgs e)
        {
            if (QuotesCommand?.CanExecute(TextBox) == true)
                QuotesCommand?.Execute(TextBox);
        }

        private void Code_Click(object sender, RoutedEventArgs e)
        {
            if (CodeCommand?.CanExecute(TextBox) == true)
                CodeCommand?.Execute(TextBox);
        }

        private void BulletPoint_Click(object sender, RoutedEventArgs e)
        {
            if (BulletPointCommand?.CanExecute(TextBox) == true)
                BulletPointCommand?.Execute(TextBox);
        }

        private void List_Click(object sender, RoutedEventArgs e)
        {
            if (ListCommand?.CanExecute(TextBox) == true)
                ListCommand?.Execute(TextBox);
        }

        private void Image_Click(object sender, RoutedEventArgs e)
        {
            var data = new DialogData(HomeViewModel, TextBox);

            if (ImageCommand?.CanExecute(data) == true)
                ImageCommand?.Execute(data);
        }

        private void Link_Click(object sender, RoutedEventArgs e)
        {
            var data = new DialogData(HomeViewModel, TextBox);

            if (LinkCommand?.CanExecute(data) == true)
            LinkCommand?.Execute(data);
        }

        private void HorizontalLine_Click(object sender, RoutedEventArgs e)
        {
            if (HorizontalLineCommand?.CanExecute(TextBox) == true)
                HorizontalLineCommand?.Execute(TextBox);
        }

        private void LineBreak_Click(object sender, RoutedEventArgs e)
        {
            if (LineBreakCommand?.CanExecute(TextBox) == true)
                LineBreakCommand?.Execute(TextBox);
        }

        private void Find_Click(object sender, RoutedEventArgs e)
        {
            if (Replace.IsChecked == true)
                Replace.IsChecked = false;

            if (ShowFindDialog != ((bool)Find.IsChecked || (bool)Replace.IsChecked))
            {
                ShowFindDialog = (bool)Find.IsChecked || (bool)Replace.IsChecked;
            }
            else if (Find.IsChecked == true || Replace.IsChecked == true)
            {
                RoutedEventArgs routedArgs = new RoutedEventArgs(FocusFindDialogEvent);
                RaiseEvent(routedArgs);
            }

            if (ShowReplace)
                ShowReplace = false;
        }

        private void Replace_Click(object sender, RoutedEventArgs e)
        {
            if (Find.IsChecked == true)
                Find.IsChecked = false;

            if (ShowFindDialog != ((bool)Find.IsChecked || (bool)Replace.IsChecked))
            {
                ShowFindDialog = (bool)Find.IsChecked || (bool)Replace.IsChecked;
            }
            else if (Find.IsChecked == true || Replace.IsChecked == true)
            {
                RoutedEventArgs routedArgs = new RoutedEventArgs(FocusFindDialogEvent);
                RaiseEvent(routedArgs);
            }

            if (!ShowReplace)
                ShowReplace = true;
        }

        private void ShowFindExec()
        {
            Find.IsChecked = true;
            Find_Click(null, null);
        }

        private void ShowReplaceExec()
        {
            Replace.IsChecked = true;
            Replace_Click(null, null);
        }
        
        private void HideFindDialogExec()
        {
            if (Find.IsChecked == true)
                Find.IsChecked = false;

            if (Replace.IsChecked == true)
                Replace.IsChecked = false;
            
            if (ShowFindDialog)
                ShowFindDialog = false;

            TextBox.Focus();
        }
    }
}

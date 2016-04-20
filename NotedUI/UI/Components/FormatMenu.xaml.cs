using ICSharpCode.AvalonEdit;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace NotedUI.UI.Components
{
    /// <summary>
    /// Interaction logic for FormatMenu.xaml
    /// </summary>
    public partial class FormatMenu : UserControl
    {
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
            if (ImageCommand?.CanExecute(TextBox) == true)
                ImageCommand?.Execute(TextBox);
        }

        private void Link_Click(object sender, RoutedEventArgs e)
        {
            if (LinkCommand?.CanExecute(TextBox) == true)
            LinkCommand?.Execute(TextBox);
        }

        private void HorizontalLine_Click(object sender, RoutedEventArgs e)
        {
            if (HorizontalLineCommand?.CanExecute(TextBox) == true)
                HorizontalLineCommand?.Execute(TextBox);
        }
    }
}

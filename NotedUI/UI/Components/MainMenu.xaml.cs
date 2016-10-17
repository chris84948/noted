using NotedUI.Models;
using NotedUI.UI.ViewModels;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace NotedUI.UI.Components
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : UserControl
    {
        public static readonly DependencyProperty AllNotesProperty =
            DependencyProperty.Register("AllNotes",
                                        typeof(AllNotesViewModel),
                                        typeof(MainMenu),
                                        new FrameworkPropertyMetadata(null));

        public AllNotesViewModel AllNotes
        {
            get { return (AllNotesViewModel)GetValue(AllNotesProperty); }
            set { SetValue(AllNotesProperty, value); }
        }

        public static readonly DependencyProperty HomeViewModelProperty =
            DependencyProperty.Register("HomeViewModel",
                                        typeof(HomeViewModel),
                                        typeof(MainMenu),
                                        new FrameworkPropertyMetadata(null));

        public HomeViewModel HomeViewModel
        {
            get { return (HomeViewModel)GetValue(HomeViewModelProperty); }
            set { SetValue(HomeViewModelProperty, value); }
        }

        public static readonly DependencyProperty ShowSearchProperty =
            DependencyProperty.Register("ShowSearch",
                                        typeof(bool),
                                        typeof(MainMenu),
                                        new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public bool ShowSearch
        {
            get { return (bool)GetValue(ShowSearchProperty); }
            set { SetValue(ShowSearchProperty, value); }
        }

        public static readonly DependencyProperty ShowPreviewProperty =
            DependencyProperty.Register("ShowPreview",
                                        typeof(bool),
                                        typeof(MainMenu),
                                        new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(ShowPreviewChangedPropertyChanged)));

        public bool ShowPreview
        {
            get { return (bool)GetValue(ShowPreviewProperty); }
            set { SetValue(ShowPreviewProperty, value); }
        }

        public static readonly RoutedEvent ShowPreviewChangedEvent =
            EventManager.RegisterRoutedEvent("ShowPreviewChanged",
                                             RoutingStrategy.Bubble,
                                             typeof(RoutedEventHandler),
                                             typeof(MainMenu));

        public event RoutedEventHandler ShowPreviewChanged
        {
            add { AddHandler(ShowPreviewChangedEvent, value); }
            remove { RemoveHandler(ShowPreviewChangedEvent, value); }
        }
        
        public static readonly DependencyProperty ShowFormatMenuProperty =
            DependencyProperty.Register("ShowFormatMenu",
                                        typeof(bool),
                                        typeof(MainMenu),
                                        new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public bool ShowFormatMenu
        {
            get { return (bool)GetValue(ShowFormatMenuProperty); }
            set { SetValue(ShowFormatMenuProperty, value); }
        }

        public static readonly DependencyProperty NotedCommandProperty =
            DependencyProperty.Register("NotedCommand",
                                        typeof(ICommand),
                                        typeof(MainMenu),
                                        new FrameworkPropertyMetadata((ICommand)null));

        [TypeConverter(typeof(CommandConverter))]
        public ICommand NotedCommand
        {
            get { return (ICommand)GetValue(NotedCommandProperty); }
            set { SetValue(NotedCommandProperty, value); }
        }

        public static readonly DependencyProperty AddNoteCommandProperty =
            DependencyProperty.Register("AddNoteCommand",
                                        typeof(ICommand),
                                        typeof(MainMenu),
                                        new PropertyMetadata((ICommand)null));

        [TypeConverter(typeof(CommandConverter))]
        public ICommand AddNoteCommand
        {
            get { return (ICommand)GetValue(AddNoteCommandProperty); }
            set { SetValue(AddNoteCommandProperty, value); }
        }

        public static readonly DependencyProperty DeleteNoteCommandProperty =
            DependencyProperty.Register("DeleteNoteCommand",
                                        typeof(ICommand),
                                        typeof(MainMenu),
                                        new PropertyMetadata((ICommand)null));

        [TypeConverter(typeof(CommandConverter))]
        public ICommand DeleteNoteCommand
        {
            get { return (ICommand)GetValue(DeleteNoteCommandProperty); }
            set { SetValue(DeleteNoteCommandProperty, value); }
        }

        public static readonly DependencyProperty GroupAddCommandProperty =
            DependencyProperty.Register("AddGroupCommand",
                                        typeof(ICommand),
                                        typeof(MainMenu),
                                        new PropertyMetadata((ICommand)null));

        [TypeConverter(typeof(CommandConverter))]
        public ICommand AddGroupCommand
        {
            get { return (ICommand)GetValue(GroupAddCommandProperty); }
            set { SetValue(GroupAddCommandProperty, value); }
        }

        public static readonly DependencyProperty ExportTextCommandProperty =
            DependencyProperty.Register("ExportTextCommand",
                                        typeof(ICommand),
                                        typeof(MainMenu),
                                        new PropertyMetadata((ICommand)null));

        [TypeConverter(typeof(CommandConverter))]
        public ICommand ExportTextCommand
        {
            get { return (ICommand)GetValue(ExportTextCommandProperty); }
            set { SetValue(ExportTextCommandProperty, value); }
        }

        public static readonly DependencyProperty ExportHTMLCommandProperty =
            DependencyProperty.Register("ExportHTMLCommand",
                                        typeof(ICommand),
                                        typeof(MainMenu),
                                        new PropertyMetadata((ICommand)null));

        [TypeConverter(typeof(CommandConverter))]
        public ICommand ExportHTMLCommand
        {
            get { return (ICommand)GetValue(ExportHTMLCommandProperty); }
            set { SetValue(ExportHTMLCommandProperty, value); }
        }

        public static readonly DependencyProperty ExportPDFCommandProperty =
            DependencyProperty.Register("ExportPDFCommand",
                                        typeof(ICommand),
                                        typeof(MainMenu),
                                        new PropertyMetadata((ICommand)null));

        [TypeConverter(typeof(CommandConverter))]
        public ICommand ExportPDFCommand
        {
            get { return (ICommand)GetValue(ExportPDFCommandProperty); }
            set { SetValue(ExportPDFCommandProperty, value); }
        }

        public static readonly DependencyProperty ShowSettingsCommandProperty =
            DependencyProperty.Register("ShowSettingsCommand",
                                        typeof(ICommand),
                                        typeof(MainMenu),
                                        new PropertyMetadata((ICommand)null));

        [TypeConverter(typeof(CommandConverter))]
        public ICommand ShowSettingsCommand
        {
            get { return (ICommand)GetValue(ShowSettingsCommandProperty); }
            set { SetValue(ShowSettingsCommandProperty, value); }
        }
        

        public MainMenu()
        {
            InitializeComponent();
        }

        private static void ShowPreviewChangedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var me = d as MainMenu;

            RoutedEventArgs args = new RoutedEventArgs(ShowPreviewChangedEvent);
            me.RaiseEvent(args);
        }
        
        private void buttonNoted_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            if (AddNoteCommand?.CanExecute(AllNotes) == true)
                AddNoteCommand?.Execute(AllNotes);
        }

        private void buttonDelete_Click(object sender, RoutedEventArgs e)
        {
            if (DeleteNoteCommand?.CanExecute(AllNotes) == true)
                DeleteNoteCommand?.Execute(AllNotes);
        }

        private void popupAddNote_Click(object sender, RoutedEventArgs e)
        {
            popupAddNote.IsOpen = false;

            var button = sender as Button;
            string groupName = button.Content.ToString();
            var cmdArgs = new AddNoteParams(AllNotes, groupName);

            if (AddNoteCommand?.CanExecute(cmdArgs) == true)
                AddNoteCommand?.Execute(cmdArgs);
        }

        private void buttonAddGroup_Click(object sender, RoutedEventArgs e)
        {
            var data = new GroupCmdParams(HomeViewModel, AllNotes);

            if (AddGroupCommand?.CanExecute(data) == true)
                AddGroupCommand?.Execute(data);
        }

        private void buttonExportText_Click(object sender, RoutedEventArgs e)
        {
            popupExport.IsOpen = false;

            if (ExportTextCommand?.CanExecute(AllNotes) == true)
                ExportTextCommand?.Execute(AllNotes);
        }

        private void buttonExportHTML_Click(object sender, RoutedEventArgs e)
        {
            popupExport.IsOpen = false;

            if (ExportHTMLCommand?.CanExecute(AllNotes) == true)
                ExportHTMLCommand?.Execute(AllNotes);
        }

        private void buttonExportPDF_Click(object sender, RoutedEventArgs e)
        {
            popupExport.IsOpen = false;

            if (ExportPDFCommand?.CanExecute(AllNotes) == true)
                ExportPDFCommand?.Execute(AllNotes);
        }

        private void buttonShowSettings_Click(object sender, RoutedEventArgs e)
        {
            if (ShowSettingsCommand?.CanExecute(HomeViewModel) == true)
                ShowSettingsCommand?.Execute(HomeViewModel);
        }
    }
}

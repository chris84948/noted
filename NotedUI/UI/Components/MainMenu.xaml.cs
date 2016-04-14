using NotedUI.Models;
using NotedUI.UI.Screens;
using NotedUI.UI.ViewModels;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System;

namespace NotedUI.UI.Components
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : UserControl
    {
        public static readonly DependencyProperty AllNotesProperty =
            DependencyProperty.Register("AllNotes",
                                        typeof(ObservableCollection<NoteViewModel>),
                                        typeof(MainMenu),
                                        new FrameworkPropertyMetadata(null));

        public ObservableCollection<NoteViewModel> AllNotes
        {
            get { return (ObservableCollection<NoteViewModel>)GetValue(AllNotesProperty); }
            set { SetValue(AllNotesProperty, value); }
        }

        public static readonly DependencyProperty SelectedNoteProperty =
            DependencyProperty.Register("SelectedNote",
                                        typeof(NoteViewModel),
                                        typeof(MainMenu),
                                        new FrameworkPropertyMetadata(null));

        public NoteViewModel SelectedNote
        {
            get { return (NoteViewModel)GetValue(SelectedNoteProperty); }
            set { SetValue(SelectedNoteProperty, value); }
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

        public static readonly DependencyProperty FolderAddCommandProperty =
            DependencyProperty.Register("FolderAddCommand",
                                        typeof(ICommand),
                                        typeof(MainMenu),
                                        new PropertyMetadata((ICommand)null));

        [TypeConverter(typeof(CommandConverter))]
        public ICommand FolderAddCommand
        {
            get { return (ICommand)GetValue(FolderAddCommandProperty); }
            set { SetValue(FolderAddCommandProperty, value); }
        }

        public static readonly DependencyProperty FolderDeleteCommandProperty =
            DependencyProperty.Register("FolderDeleteCommand",
                                        typeof(ICommand),
                                        typeof(MainMenu),
                                        new PropertyMetadata((ICommand)null));

        [TypeConverter(typeof(CommandConverter))]
        public ICommand FolderDeleteCommand
        {
            get { return (ICommand)GetValue(FolderDeleteCommandProperty); }
            set { SetValue(FolderDeleteCommandProperty, value); }
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

        public static readonly RoutedEvent SettingsClickedEvent =
            EventManager.RegisterRoutedEvent("SettingsClicked",
                                             RoutingStrategy.Bubble,
                                             typeof(RoutedEventHandler),
                                             typeof(MainMenu));

        public event RoutedEventHandler SettingsClicked
        {
            add { AddHandler(SettingsClickedEvent, value); }
            remove { RemoveHandler(SettingsClickedEvent, value); }
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

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            if (AddNoteCommand?.CanExecute(AllNotes) == true)
                AddNoteCommand?.Execute(AllNotes);
        }

        private void buttonDelete_Click(object sender, RoutedEventArgs e)
        {
            var data = new ListData(AllNotes, SelectedNote);

            if (DeleteNoteCommand?.CanExecute(data) == true)
                DeleteNoteCommand?.Execute(data);
        }

        private void buttonAddFolder_Click(object sender, RoutedEventArgs e)
        {
            popupFolder.IsOpen = false;

            if (FolderAddCommand?.CanExecute(AllNotes) == true)
                FolderAddCommand?.Execute(AllNotes);
        }

        private void buttonDeleteFolder_Click(object sender, RoutedEventArgs e)
        {
            popupFolder.IsOpen = false;

            if (FolderDeleteCommand?.CanExecute(AllNotes) == true)
                FolderDeleteCommand?.Execute(AllNotes);
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
            RoutedEventArgs args = new RoutedEventArgs(SettingsClickedEvent);
            RaiseEvent(args);
        }
    }
}

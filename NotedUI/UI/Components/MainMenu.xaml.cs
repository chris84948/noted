using NotedUI.UI.Screens;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
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

namespace NotedUI.UI.Components
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : UserControl
    {
        public static readonly DependencyProperty AllNotesProperty =
            DependencyProperty.Register("AllNotes",
                                        typeof(ObservableCollection<Note>),
                                        typeof(MainMenu),
                                        new FrameworkPropertyMetadata(null));

        public ObservableCollection<Note> AllNotes
        {
            get { return (ObservableCollection<Note>)GetValue(AllNotesProperty); }
            set { SetValue(AllNotesProperty, value); }
        }

        public static readonly DependencyProperty SelectedNoteProperty =
            DependencyProperty.Register("SelectedNote",
                                        typeof(Note),
                                        typeof(MainMenu),
                                        new FrameworkPropertyMetadata(null));

        public Note SelectedNote
        {
            get { return (Note)GetValue(SelectedNoteProperty); }
            set { SetValue(SelectedNoteProperty, value); }
        }

        public static readonly DependencyProperty ShowSearchProperty =
            DependencyProperty.Register("ShowSearch",
                                        typeof(bool),
                                        typeof(MainMenu),
                                        new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public bool ShowSearch { get { return (bool)GetValue(ShowSearchProperty); } set { SetValue(ShowSearchProperty, value); } }

        public static readonly DependencyProperty ShowPreviewProperty =
            DependencyProperty.Register("ShowPreview",
                                        typeof(bool),
                                        typeof(MainMenu),
                                        new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public bool ShowPreview { get { return (bool)GetValue(ShowPreviewProperty); } set { SetValue(ShowPreviewProperty, value); } }

        public static readonly DependencyProperty ShowFormatMenuProperty =
            DependencyProperty.Register("ShowFormatMenu",
                                        typeof(bool),
                                        typeof(MainMenu),
                                        new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public bool ShowFormatMenu { get { return (bool)GetValue(ShowFormatMenuProperty); } set { SetValue(ShowFormatMenuProperty, value); } }

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

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            AddNoteCommand?.Execute(AllNotes);
        }

        private void buttonDelete_Click(object sender, RoutedEventArgs e)
        {
            DeleteNoteCommand?.Execute(new ListData(AllNotes, SelectedNote));
        }

        private void buttonAddFolder_Click(object sender, RoutedEventArgs e)
        {
            popupFolder.IsOpen = false;
            FolderAddCommand?.Execute(AllNotes);
        }

        private void buttonDeleteFolder_Click(object sender, RoutedEventArgs e)
        {
            popupFolder.IsOpen = false;
            FolderDeleteCommand?.Execute(AllNotes);
        }

        private void buttonExportHTML_Click(object sender, RoutedEventArgs e)
        {
            popupExport.IsOpen = false;
            ExportHTMLCommand?.Execute(AllNotes);
        }

        private void buttonExportPDF_Click(object sender, RoutedEventArgs e)
        {
            popupExport.IsOpen = false;
            ExportPDFCommand?.Execute(AllNotes);
        }

        private void buttonShowSettings_Click(object sender, RoutedEventArgs e)
        {
            ShowSettingsCommand?.Execute(AllNotes);
        }
    }
}

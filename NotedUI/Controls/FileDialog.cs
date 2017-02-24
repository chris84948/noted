using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using NotedUI.Models;
using System.Threading.Tasks;

namespace NotedUI.Controls
{
    public class FileDialog : Control
    {
        private TreeView _folderTreeView;
        private ListView _filesListView;

        private string _startingPath;
        private Stack<string> _previousPaths = new Stack<string>();
        private Stack<string> _nextPaths = new Stack<string>();
        private bool _updatePreviousStack = false;
        private bool _updateFolderLocation = true;


        // ROUTED EVENTS
        public static readonly RoutedEvent FileDoubleClickedEvent =
            EventManager.RegisterRoutedEvent("FileDoubleClicked",
                                             RoutingStrategy.Bubble,
                                             typeof(FileDoubleClickedRoutedEventHandler),
                                             typeof(FileDialog));

        public event FileDoubleClickedRoutedEventHandler FileDoubleClicked
        {
            add { AddHandler(FileDoubleClickedEvent, value); }
            remove { RemoveHandler(FileDoubleClickedEvent, value); }
        }


        // DEPENDENCY PROPERTIES
        public static readonly DependencyProperty PathProperty =
            DependencyProperty.Register("Path",
                                        typeof(string),
                                        typeof(FileDialog),
                                        new FrameworkPropertyMetadata(null,
                                                                      FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                                                                      new PropertyChangedCallback(OnPathChanged)));

        public string Path
        {
            get { return (string)GetValue(PathProperty); }
            set { SetValue(PathProperty, value); }
        }

        public static readonly DependencyProperty SelectedPathProperty =
            DependencyProperty.Register("SelectedPath",
                                        typeof(DirectoryItem),
                                        typeof(FileDialog),
                                        new FrameworkPropertyMetadata(null));
        
        public DirectoryItem SelectedPath
        {
            get { return (DirectoryItem)GetValue(SelectedPathProperty); }
            set { SetValue(SelectedPathProperty, value); }
        }

        public static readonly DependencyProperty ShowFilesProperty =
            DependencyProperty.Register("ShowFiles",
                                        typeof(bool),
                                        typeof(FileDialog),
                                        new FrameworkPropertyMetadata(true));

        public bool ShowFiles
        {
            get { return (bool)GetValue(ShowFilesProperty); }
            set { SetValue(ShowFilesProperty, value); }
        }

        public static readonly DependencyProperty FileFilterProperty =
            DependencyProperty.Register("FileFilter",
                                        typeof(string),
                                        typeof(FileDialog),
                                        new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnFileFilterChanged)));

        public string FileFilter
        {
            get { return (string)GetValue(FileFilterProperty); }
            set { SetValue(FileFilterProperty, value); }
        }


        public static readonly DependencyProperty FolderWidthProperty =
            DependencyProperty.Register("FolderWidth",
                                        typeof(double),
                                        typeof(FileDialog),
                                        new FrameworkPropertyMetadata(200.0));

        public double FolderWidth
        {
            get { return (double)GetValue(FolderWidthProperty); }
            set { SetValue(FolderWidthProperty, value); }
        }

        private static readonly DependencyPropertyKey SelectedFolderFilesPropertyKey =
            DependencyProperty.RegisterReadOnly("SelectedFolderFiles",
                                                typeof(List<DirectoryItem>),
                                                typeof(FileDialog),
                                                new PropertyMetadata(null));

        public static readonly DependencyProperty SelectedFolderFilesProperty = SelectedFolderFilesPropertyKey.DependencyProperty;
        public List<DirectoryItem> SelectedFolderFiles
        {
            get { return (List<DirectoryItem>)GetValue(SelectedFolderFilesProperty); }
        }

        private List<string> _fileFilters;

        public FileDialog()
        {
        }

        static FileDialog()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FileDialog), new FrameworkPropertyMetadata(typeof(FileDialog)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _filesListView = GetTemplateChild("PART_Files") as ListView;
            _filesListView.MouseDoubleClick += FilesListView_MouseDoubleClick;
            
            _folderTreeView = GetTemplateChild("PART_FolderTreeView") as TreeView;

            _folderTreeView.SelectedItemChanged += FolderTreeView_SelectedItemChanged;
            _folderTreeView.AddHandler(TreeViewItem.ExpandedEvent, new RoutedEventHandler(FolderTreeView_Expanded));

            this.MouseUp += FileDialog_MouseUp;

            Task.Run(() => GetDrivesAsync());
        }

        private static void OnFileFilterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null)
                return;

            var dialog = d as FileDialog;

            dialog._fileFilters = new List<string>();
            foreach (var fileFilter in e.NewValue.ToString().Split(','))
                dialog._fileFilters.Add(fileFilter.StartsWith(".") ? fileFilter.ToUpper() : "." + fileFilter.ToUpper());
        }

        private void FileDialog_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.XButton1 && _previousPaths.Count > 0)
            {
                // First put the current path in the previous stack
                _nextPaths.Push(Path);

                // Now jump to the next path on the stack
                _updatePreviousStack = false;
                SelectFolder(_folderTreeView, _previousPaths.Pop());
            }
            else if (e.ChangedButton == MouseButton.XButton2 && _nextPaths.Count > 0)
            {
                // First put the current path in the previous stack
                _previousPaths.Push(Path);

                // Now jump to the next path on the stack
                _updatePreviousStack = false;
                SelectFolder(_folderTreeView, _nextPaths.Pop());
            }
        }

        private void GetDrivesAsync()
        {
            var drives = new List<Folder>();

            drives.Add(new Folder(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Links"), null));
            foreach (var drive in Directory.GetLogicalDrives())
                drives.Add(new Folder(drive, null));

            App.Current.Dispatcher.Invoke(() =>
            {
                foreach (var folder in drives)
                    _folderTreeView.Items.Add(folder);

                if (_startingPath != null)
                    SelectFolder(_folderTreeView, _startingPath);
            });
        }

        private static void OnPathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var fileDialog = d as FileDialog;
            string newVal = (string)e.NewValue;

            if (fileDialog.IsLoaded)
            {
                if (fileDialog._updateFolderLocation)
                    fileDialog.SelectFolder(fileDialog._folderTreeView, newVal);
            }
            else
            {
                fileDialog._startingPath = newVal;
            }
        }

        private void FolderTreeView_Expanded(object sender, RoutedEventArgs e)
        {
            var folder = (e.OriginalSource as TreeViewItem).Header as Folder;
            folder.ReadSubItemsForFolder();
        }

        private void FolderTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var folder = e.NewValue as Folder;

            if (_updatePreviousStack && folder.Path != Path)
                _previousPaths.Push(Path);
            else
                _updatePreviousStack = true;

            // The user selected the folder location, don't force it to update again
            _updateFolderLocation = false;
            SetValue(PathProperty, folder.Path);
            SetValue(SelectedFolderFilesPropertyKey, GetFolderFiles(folder.Path));
            _updateFolderLocation = true;
        }

        private void FilesListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = (sender as ListView).SelectedItem as DirectoryItem;

            if (item.IsFolder)
            {
                SelectFolder(_folderTreeView, item.Path);
            }
            else
            {
                // Raise event to subscribers that a file has been double clicked/selected
                var args = new FileDoubleClickedRoutedEventArgs(FileDoubleClickedEvent, item.Path);
                RaiseEvent(args);
            }
        }
        
        public List<DirectoryItem> GetFolderFiles(string path)
        {
            var allItems = new List<DirectoryItem>();

            foreach (var info in new DirectoryInfo(path).EnumerateFileSystemInfos("*", SearchOption.TopDirectoryOnly)
                                                        .Where(i => ((i.Attributes & FileAttributes.Directory) == FileAttributes.Directory) ||
                                                                    _fileFilters?.Any(f => f == i.Extension.ToUpper()) == true))
            {
                if (info.Extension == ".lnk")
                    allItems.Add(new DirectoryItem(LinkConverter.GetLnkTarget(info.FullName), isFolder: true));
                else if ((info.Attributes & FileAttributes.Directory) == FileAttributes.Directory)
                    allItems.Add(new DirectoryItem(info.FullName, isFolder: true));
                else if (ShowFiles)
                    allItems.Add(new DirectoryItem(info.FullName, isFolder: false));
            }

            return allItems.OrderBy(x => !x.IsFolder).ToList();
        }

        /// <summary>
        /// Selects the correct path from the treeview
        /// </summary>
        /// <param name="targetFolder">Desired path</param>
        private void SelectFolder(TreeView treeView, string targetFolder)
        {
            // Don't try and find the folder if it's empty
            if (string.IsNullOrEmpty(targetFolder))
                return;

            // Loop through all drives
            foreach (Folder folder in treeView.Items)
            {
                if (targetFolder.StartsWith(folder.Path))
                {
                    TreeViewItem item = treeView.ItemContainerGenerator.ContainerFromItem(folder) as TreeViewItem;
                    RecursivelySelectFolder(treeView, targetFolder + "\\", item);
                    return;
                }
                else if (folder.Name == "Favorites")
                {
                    TreeViewItem item = treeView.ItemContainerGenerator.ContainerFromItem(folder) as TreeViewItem;
                    item.IsExpanded = true;
                    item.UpdateLayout();

                    foreach (var subFolder in folder.SubItems.Where(f => f != null))
                    {
                        if (targetFolder.StartsWith(subFolder.Path))
                        {
                            TreeViewItem subItem = item.ItemContainerGenerator.ContainerFromItem(subFolder) as TreeViewItem;
                            RecursivelySelectFolder(treeView, targetFolder + "\\", subItem);
                            return;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Recursively loops through the treeview structure to get the correct
        /// folder with the right path
        /// </summary>
        /// <param name="targetPath">Desired path</param>
        /// <param name="tvItem">Current treeview item</param>
        private static void RecursivelySelectFolder(TreeView treeView, string targetPath, TreeViewItem tvItem)
        {
            Folder thisFolder = tvItem.DataContext as Folder;

            // We found this item - select it
            if (targetPath.Equals(thisFolder.Path, StringComparison.CurrentCultureIgnoreCase) ||
                targetPath.Equals(thisFolder.Path + "\\", StringComparison.CurrentCultureIgnoreCase))
            {
                tvItem.IsSelected = true;
                tvItem.BringIntoView();
                treeView.Focus();
            }
            // We're on the right path, keep moving down a level
            else if (targetPath.StartsWith(thisFolder.Path + (thisFolder.IsDrive ? "" : "\\"), StringComparison.CurrentCultureIgnoreCase))
            {
                thisFolder.ReadSubItemsForFolder();
                tvItem.IsExpanded = true;
                treeView.UpdateLayout();

                foreach (Folder subFolder in tvItem.Items)
                {
                    TreeViewItem subItem = tvItem.ItemContainerGenerator.ContainerFromItem(subFolder) as TreeViewItem;
                    RecursivelySelectFolder(treeView, targetPath, subItem);
                }
            }
            else // This isn't it, break the tree
            {
                tvItem.IsExpanded = false;
                treeView.UpdateLayout();
                return;
            }
        }
    }
}

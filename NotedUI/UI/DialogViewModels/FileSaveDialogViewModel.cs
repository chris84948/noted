using JustMVVM;
using NotedUI.Models;
using NotedUI.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NotedUI.UI.DialogViewModels
{
    public class FileSaveDialogViewModel : MVVMBase, IDialog
    {
        public event Action<IDialog> DialogClosed;
        public ICommand OKCommand { get { return new RelayCommand(OKExec, CanOKExec); } }
        public ICommand CancelCommand { get { return new RelayCommand(CancelExec); } }
        
        private string _fileType;
        public string FileType
        {
            get { return _fileType; }
            set
            {
                _fileType = value;
                OnPropertyChanged();
            }
        }

        private string _fileFilter;
        public string FileFilter
        {
            get { return _fileFilter; }
            set
            {
                _fileFilter = value;
                OnPropertyChanged();
            }
        }

        private string _path;
        public string Path
        {
            get { return _path; }
            set
            {
                _path = value;
                OnPropertyChanged();
            }
        }

        private DirectoryItem _selectedFile;
        public DirectoryItem SelectedFile
        {
            get { return _selectedFile; }
            set
            {
                _selectedFile = value;
                OnPropertyChanged();

                if (_selectedFile?.IsFolder == false)
                    Filename = System.IO.Path.GetFileName(_selectedFile.Path);
            }
        }

        private string _filename;
        public string Filename
        {
            get { return _filename; }
            set
            {
                _filename = value;
                OnPropertyChanged();
            }
        }

        private IDialog _dialog;
        public IDialog Dialog
        {
            get { return _dialog; }
            set
            {
                _dialog = value;
                OnPropertyChanged();
            }
        }

        private bool _showDialog;
        public bool ShowDialog
        {
            get { return _showDialog; }
            set
            {
                _showDialog = value;
                OnPropertyChanged();
            }
        }

        public System.Windows.Forms.DialogResult Result { get; set; }
        public string ResultFilename { get; set; }

        public FileSaveDialogViewModel(string fileFilter, string path = null)
        {
            FileType = fileFilter.ToUpper();
            FileFilter = fileFilter;

            if (path == null)
            {
                Path = "C:\\";
            }
            else
            {
                Path = System.IO.Path.GetDirectoryName(path);
                Filename = System.IO.Path.GetFileNameWithoutExtension(path);
            }

        }

        private bool CanOKExec()
        {
            return !String.IsNullOrWhiteSpace(Filename);
        }

        private void OKExec()
        {
            ResultFilename = System.IO.Path.Combine(Path, Filename);

            if (System.IO.File.Exists(ResultFilename))
            {
                var dialog = new YesNoDialogViewModel("Do you want to overwrite this file?");
                dialog.DialogClosed += (d) =>
                {
                    if (dialog.Result == System.Windows.Forms.DialogResult.Yes)
                    {
                        ShowDialog = false;
                        Dialog = null;
                        GetResultAndCloseDialog();
                    }
                };

                Dialog = dialog;
                ShowDialog = true;
            }
            else
            {
                GetResultAndCloseDialog();
            }
        }

        private void GetResultAndCloseDialog()
        {
            if (!ResultFilename.EndsWith("." + FileFilter))
                ResultFilename = ResultFilename + "." + FileFilter.ToLower();

            Result = System.Windows.Forms.DialogResult.OK;
            DialogClosed?.Invoke(this);
        }

        private void CancelExec()
        {
            Result = System.Windows.Forms.DialogResult.Cancel;
            DialogClosed?.Invoke(this);
        }
    }
}

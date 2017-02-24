using JustMVVM;
using NotedUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NotedUI.UI.ViewModels
{
    public class FileSaveDialogViewModel : MVVMBase, IDialog
    {
        public event Action DialogClosed;
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

        public System.Windows.Forms.DialogResult Result { get; set; }
        public string ResultFilename { get; set; }

        public FileSaveDialogViewModel(string fileFilter, string path = null)
            : this(fileFilter.ToUpper(), fileFilter, path)
        { }

        public FileSaveDialogViewModel(string fileType, string fileFilter, string path = null)
        {
            FileType = fileType.ToUpper();
            FileFilter = fileFilter;
            Path = path ?? "C:\\";
        }

        private bool CanOKExec()
        {
            return !String.IsNullOrWhiteSpace(Filename);
        }

        private void OKExec()
        {
            ResultFilename = System.IO.Path.Combine(Path, Filename);

            if (!ResultFilename.EndsWith(FileType))
                ResultFilename = ResultFilename + "." + FileType.ToLower();

            Result = System.Windows.Forms.DialogResult.OK;
            DialogClosed?.Invoke();
        }

        private void CancelExec()
        {
            Result = System.Windows.Forms.DialogResult.Cancel;
            DialogClosed?.Invoke();
        }
    }
}

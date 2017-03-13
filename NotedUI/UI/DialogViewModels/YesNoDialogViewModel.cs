using JustMVVM;
using NotedUI.Models;
using NotedUI.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace NotedUI.UI.DialogViewModels
{
    public class YesNoDialogViewModel : MVVMBase, IDialog
    {
        public event Action<IDialog> DialogClosed;
        public ICommand YesCommand { get { return new RelayCommand(YesExec); } }
        public ICommand NoCommand { get { return new RelayCommand(NoExec); } }

        public System.Windows.Forms.DialogResult Result { get; set; }

        private string _title;
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        private string _yesText;
        public string YesText
        {
            get { return _yesText; }
            set
            {
                _yesText = value;
                OnPropertyChanged();
            }
        }

        private string _noText;
        public string NoText
        {
            get { return _noText; }
            set
            {
                _noText = value;
                OnPropertyChanged();
            }
        }

        public YesNoDialogViewModel(string dialogTitle, string yesText = "YES", string noText = "NO")
        {
            Title = dialogTitle;
            YesText = yesText;
            NoText = noText;
        }

        private void YesExec()
        {
            Result = System.Windows.Forms.DialogResult.Yes;
            DialogClosed?.Invoke(this);
        }

        private void NoExec()
        {
            Result = System.Windows.Forms.DialogResult.No;
            DialogClosed?.Invoke(this);
        }
    }
}

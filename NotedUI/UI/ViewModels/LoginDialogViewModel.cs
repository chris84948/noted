using JustMVVM;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace NotedUI.UI.ViewModels
{
    public class LoginDialogViewModel : MVVMBase, IDialog
    {
        public event Action<IDialog> DialogClosed;
        public ICommand OKCommand { get { return new RelayCommand(OKExec); } }
        public ICommand CancelCommand { get { return new RelayCommand(CancelExec); } }

        private string _username;
        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                OnPropertyChanged();

                ShowError = false;
            }
        }

        private bool _isAuthorizing;
        public bool IsAuthorizing
        {
            get { return _isAuthorizing; }
            set
            {
                _isAuthorizing = value;
                OnPropertyChanged();
            }
        }

        private string _errorMessage = null;
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                OnPropertyChanged();
            }
        }

        private bool _showError;
        public bool ShowError
        {
            get { return _showError; }
            set
            {
                _showError = value;
                OnPropertyChanged();
            }
        }

        public LoginDialogViewModel()
        {
        }

        private void OKExec()
        {
            Task.Run(async () => await OKExecAsync());
        }

        private async Task OKExecAsync()
        {
            IsAuthorizing = true;

            if (await App.Cloud.Connect(Username))
            {
                await App.Local.InsertUsername(Username);
                DialogClosed?.Invoke(this);
            }
            else
            {
                ErrorMessage = $"Google Drive authorization failed for { Username }.";
                ShowError = true;
            }

            IsAuthorizing = false;
        }

        private void CancelExec()
        {
            Application.Current.Shutdown();
        }
    }
}

using JustMVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NotedUI.UI.ViewModels
{
    public class SettingsCloudViewModel : MVVMBase
    {
        public ICommand ChangeUserCommand { get { return new RelayCommand(ChangeUserExec); } }

        private string _username;
        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }

        public SettingsCloudViewModel()
        {
            Username = App.Local.GetUsername().Result;
        }

        public void ChangeUserExec()
        {
            App.Local.DeleteDatabase();
            App.Cloud.DeleteCredentials(Username);

            App.RestartApplication();
        }
    }
}

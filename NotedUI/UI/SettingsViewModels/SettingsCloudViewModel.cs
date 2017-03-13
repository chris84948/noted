using JustMVVM;
using NotedUI.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NotedUI.UI.SettingsViewModels
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

        private HomeViewModel _homeVM;

        public SettingsCloudViewModel(HomeViewModel homeVM)
        {
            Username = App.Local.GetUsername().Result;
            _homeVM = homeVM;
        }

        public void ChangeUserExec()
        {
            App.Local.DeleteDatabase();
            _homeVM.Close();
            App.RestartApplication();
        }
    }
}

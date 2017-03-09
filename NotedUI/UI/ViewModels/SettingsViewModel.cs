using JustMVVM;
using NotedUI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NotedUI.UI.ViewModels
{
    class SettingsViewModel : MVVMBase, IScreen
    {
        public ICommand CloseCommand { get { return new RelayCommand(CloseExec); } }

        private List<string> _settingsNameCollection;
        public List<string> SettingsNameCollection
        {
            get { return _settingsNameCollection; }
            set
            {
                _settingsNameCollection = value;
                OnPropertyChanged();
            }
        }

        private string _selectedSettingsName;
        public string SelectedSettingsName
        {
            get { return _selectedSettingsName; }
            set
            {
                _selectedSettingsName = value;
                OnPropertyChanged();

                ChangeView(_selectedSettingsName);
            }
        }

        private MVVMBase _settingsView;
        public MVVMBase SettingsView
        {
            get { return _settingsView; }
            set
            {
                _settingsView = value;
                OnPropertyChanged();
            }
        }

        public event Action<IScreen, eTransitionType> ChangeScreen;

        private HomeViewModel _homeVM;
        private Dictionary<string, MVVMBase> _settingsScreens;

        public SettingsViewModel(HomeViewModel homeVM)
        {
            _homeVM = homeVM;

            _settingsScreens = new Dictionary<string, MVVMBase>()
            {
                { "CLOUD SETTINGS", new SettingsCloudViewModel(_homeVM) },
                { "ABOUT NOTED", new SettingsAboutViewModel() },
            };

            SelectedSettingsName = "CLOUD SETTINGS";

            SettingsNameCollection = _settingsScreens.Keys.ToList();
        }

        private void ChangeView(string settingsName)
        {
            if (_settingsScreens.ContainsKey(settingsName))
                SettingsView = _settingsScreens[settingsName];
        }

        private void CloseExec()
        {
            Task.Run(async () =>
            {
                ChangeScreen?.Invoke(_homeVM, eTransitionType.SlideOutFromRight);

                await Task.Delay(500);
                _homeVM.FixAirspace = false;
            });
        }

        private void Close()
        {

        }
    }
}
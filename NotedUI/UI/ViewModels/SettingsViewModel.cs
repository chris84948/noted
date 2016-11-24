using JustMVVM;
using NotedUI.Controls;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NotedUI.UI.ViewModels
{
    class SettingsViewModel : MVVMBase, IScreen
    {
        public event Action<IScreen, eTransitionType> ChangeScreen;

        public ICommand CloseCommand { get { return new RelayCommand(CloseExec); } }

        private HomeViewModel _homeVM;

        public SettingsViewModel(HomeViewModel homeVM)
        {
            _homeVM = homeVM;
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
    }
}

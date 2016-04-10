using JustMVVM;
using NotedUI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            ChangeScreen?.Invoke(_homeVM, eTransitionType.SlideOutFromRight);
        }
    }
}

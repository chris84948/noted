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
    class SettingsViewModel : MVVMBase
    {
        private HomeViewModel _homeVM;

        public SettingsViewModel(HomeViewModel homeVM)
        {
            _homeVM = homeVM;
        }
    }
}

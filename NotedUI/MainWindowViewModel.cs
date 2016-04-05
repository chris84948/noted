using JustMVVM;
using NotedUI.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotedUI
{
    class MainWindowViewModel : MVVMBase
    {
        private MVVMBase _currentViewModel;
        public MVVMBase CurrentViewModel
        {
            get { return _currentViewModel; }
            set
            {
                _currentViewModel = value;
                OnPropertyChanged();
            }
        }

        public MainWindowViewModel()
        {
            CurrentViewModel = new HomeViewModel();
        }
    }
}

using JustMVVM;
using NotedUI.Controls;
using NotedUI.UI.ViewModels;

namespace NotedUI.UI.ViewModels
{
    class MainWindowViewModel : MVVMBase
    {
        private IScreen _currentViewModel;
        public IScreen CurrentViewModel
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

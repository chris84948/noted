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

        private Transition _transition;
        public Transition Transition
        {
            get { return _transition; }
            set
            {
                _transition = value;
                OnPropertyChanged();
            }
        }

        public MainWindowViewModel()
        {
            CurrentViewModel = new HomeViewModel();
            CurrentViewModel.ChangeScreen += CurrentViewModel_ChangeScreen;
        }

        private void CurrentViewModel_ChangeScreen(IScreen screen, eTransitionType transitionType)
        {
            CurrentViewModel.ChangeScreen -= CurrentViewModel_ChangeScreen;

            Transition = TransitionSelector.Get(transitionType);
            CurrentViewModel = screen;

            CurrentViewModel.ChangeScreen += CurrentViewModel_ChangeScreen;
        }
    }
}

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

        private IDialog _dialog;
        public IDialog Dialog
        {
            get { return _dialog; }
            set
            {
                _dialog = value;
                OnPropertyChanged();
            }
        }

        private bool _showDialog;
        public bool ShowDialog
        {
            get { return _showDialog; }
            set
            {
                _showDialog = value;
                OnPropertyChanged();
            }
        }

        public MainWindowViewModel()
        {
            CurrentViewModel = new HomeViewModel();
            CurrentViewModel.ChangeScreen += CurrentViewModel_ChangeScreen;
            (CurrentViewModel as HomeViewModel).ShowDialog += CurrentViewModel_ShowDialog;
        }

        private void CurrentViewModel_ChangeScreen(IScreen screen, eTransitionType transitionType)
        {
            CurrentViewModel.ChangeScreen -= CurrentViewModel_ChangeScreen;

            if (CurrentViewModel is HomeViewModel)
                (CurrentViewModel as HomeViewModel).ShowDialog -= CurrentViewModel_ShowDialog;

            Transition = TransitionSelector.Get(transitionType);
            CurrentViewModel = screen;

            CurrentViewModel.ChangeScreen += CurrentViewModel_ChangeScreen;
            if (CurrentViewModel is HomeViewModel)
                (CurrentViewModel as HomeViewModel).ShowDialog += CurrentViewModel_ShowDialog;
        }

        private void CurrentViewModel_ShowDialog(IDialog dialog)
        {
            Dialog = dialog;
            Dialog.DialogClosed += () =>
            {
                ShowDialog = false;
            };
            ShowDialog = true;
        }
    }
}

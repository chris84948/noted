using JustMVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace NotedUI.UI.ViewModels
{
    public class GroupNameDialogViewModel : MVVMBase, IDialog
    {
        public event Action DialogClosed;
        public ICommand OKCommand { get { return new RelayCommand(OKExec, CanOKExec); } }
        public ICommand CancelCommand { get { return new RelayCommand(CancelExec); } }

        public System.Windows.Forms.DialogResult Result { get; set; }

        private List<string> _allGroups;

        private string _groupName;
        public string GroupName
        {
            get { return _groupName; }
            set
            {
                _groupName = value;
                OnPropertyChanged();
            }
        }

        private bool _showWarning;
        public bool ShowWarning
        {
            get { return _showWarning; }
            set
            {
                _showWarning = value;
                OnPropertyChanged();
            }
        }

        public GroupNameDialogViewModel(List<GroupViewModel> allGroups)
            : this(allGroups, "")
        { }

        public GroupNameDialogViewModel(List<GroupViewModel> allGroups, string groupName)
        {
            _allGroups = allGroups.Select(x => x.Name).ToList();
            GroupName = groupName;
        }

        private bool CanOKExec()
        {
            ShowWarning = _allGroups.Contains(GroupName.ToUpper());

            return !String.IsNullOrWhiteSpace(GroupName) && !ShowWarning;
        }

        private void OKExec()
        {
            Result = System.Windows.Forms.DialogResult.OK;
            DialogClosed?.Invoke();
        }

        private void CancelExec()
        {
            Result = System.Windows.Forms.DialogResult.Cancel;
            DialogClosed?.Invoke();
        }
    }
}

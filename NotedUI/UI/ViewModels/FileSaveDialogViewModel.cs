using JustMVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NotedUI.UI.ViewModels
{
    public class FileSaveDialogViewModel : MVVMBase, IDialog
    {
        public event Action DialogClosed;
        public ICommand OKCommand { get { return new RelayCommand(OKExec); } }
        public ICommand CancelCommand { get { return new RelayCommand(CancelExec); } }

        public System.Windows.Forms.DialogResult Result { get; set; }

        public FileSaveDialogViewModel()
        {

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

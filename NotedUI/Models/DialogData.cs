using ICSharpCode.AvalonEdit;
using NotedUI.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotedUI.Models
{
    public class DialogData
    {
        public HomeViewModel HomeVM { get; set; }
        public TextEditor Editor { get; set; }

        public DialogData() { }

        public DialogData(HomeViewModel homeVM, TextEditor editor)
        {
            HomeVM = homeVM;
            Editor = editor;
        }
    }
}

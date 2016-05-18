using ICSharpCode.AvalonEdit;
using NotedUI.UI.ViewModels;

namespace NotedUI.Models
{
    public class AddLinkParams
    {
        public HomeViewModel HomeVM { get; set; }
        public TextEditor Editor { get; set; }

        public AddLinkParams() { }

        public AddLinkParams(HomeViewModel homeVM, TextEditor editor)
        {
            HomeVM = homeVM;
            Editor = editor;
        }
    }
}

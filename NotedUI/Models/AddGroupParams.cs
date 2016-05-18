using NotedUI.UI.ViewModels;

namespace NotedUI.Models
{
    public class AddGroupParams
    {
        public HomeViewModel HomeVM { get; set; }
        public AllNotesViewModel AllNotes { get; set; }

        public AddGroupParams() { }

        public AddGroupParams(HomeViewModel homeVM, AllNotesViewModel allNotes)
        {
            HomeVM = homeVM;
            AllNotes = allNotes;
        }
    }
}

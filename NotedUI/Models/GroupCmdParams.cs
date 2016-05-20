using NotedUI.UI.ViewModels;

namespace NotedUI.Models
{
    public class GroupCmdParams
    {
        public HomeViewModel HomeVM { get; set; }
        public AllNotesViewModel AllNotes { get; set; }
        public string GroupName { get; set; }

        public GroupCmdParams() { }

        public GroupCmdParams(HomeViewModel homeVM, AllNotesViewModel allNotes, string groupName = null)
        {
            HomeVM = homeVM;
            AllNotes = allNotes;
            GroupName = groupName;
        }
    }
}

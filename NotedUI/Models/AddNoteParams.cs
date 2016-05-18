using NotedUI.UI.ViewModels;
using System.Collections.ObjectModel;

namespace NotedUI.Models
{
    public class AddNoteParams
    {
        public AllNotesViewModel AllNotes { get; set; }
        public string GroupName { get; set; }

        public AddNoteParams(AllNotesViewModel allNotes, string groupName)
        {
            AllNotes = allNotes;
            GroupName = groupName;
        }
    }
}

using JustMVVM;
using NotedUI.Models;

namespace NotedUI.UI.ViewModels
{
    public class GroupViewModel : MVVMBase
    {
        /// <summary>
        /// Underlying object for writing/reading from SQL
        /// </summary>
        internal Group GroupData { get; set; } = new Group();

        public long ID
        {
            get { return GroupData.ID; }
            set
            {
                GroupData.ID = value;
                OnPropertyChanged();
            }
        }

        public string Name
        {
            get { return GroupData.Name; }
            set
            {
                GroupData.Name = value.ToUpper();
                OnPropertyChanged();
            }
        }

        public GroupViewModel()
        { }

        public GroupViewModel(Group group)
            : this(group.ID, group.Name)
        { }

        public GroupViewModel(long id, string name)
        {
            ID = id;
            Name = name;
        }
    }
}

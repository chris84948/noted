using JustMVVM;
using Newtonsoft.Json;
using NotedUI.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace NotedUI.Models
{
    public class GroupCollection : MVVMBase
    {
        private ObservableCollection<GroupViewModel> _groups;
        public ObservableCollection<GroupViewModel> Groups
        {
            get { return _groups; }
            set
            {
                _groups = value;
                OnPropertyChanged();
            }
        }

        private DateTime _lastModified;
        [JsonIgnore]
        public DateTime LastModified
        {
            get { return _lastModified; }
            set
            {
                _lastModified = value;
                OnPropertyChanged();
            }
        }

        public GroupCollection() 
            : this(new List<GroupViewModel>())
        { }

        public GroupCollection(IEnumerable<GroupViewModel> groups)
        {
            Groups = new ObservableCollection<GroupViewModel>(groups);
        }

        public string ToJSON()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static GroupCollection FromJSON(string jsonString)
        {
            return JsonConvert.DeserializeObject<GroupCollection>(jsonString);
        }

        public void Add(ICollectionView view, GroupViewModel group)
        {
            Groups.Add(group);
            UpdateViewGroupDescription(view);
        }

        public void Update(ICollectionView view, string oldGroupName, string newGroupName)
        {
            // Update the groups list
            foreach (var group in Groups)
            {
                if (group.Name.ToUpper() == oldGroupName.ToUpper())
                    group.Name = newGroupName;
            }

            UpdateViewGroupDescription(view);
            App.Current.Dispatcher.Invoke(() => view.Refresh());
        }

        public void Delete(ICollectionView view, string groupName)
        {
            // Reverse through the list of groups to delete it
            for (int i = 0; i < Groups.Count; i++)
            {
                if (Groups[i].Name.ToUpper() == groupName.ToUpper())
                {
                    Groups.RemoveAt(i);
                    UpdateViewGroupDescription(view);
                    return;
                }
            }
        }

        public void UpdateViewGroupDescription(ICollectionView view)
        {
            if (Groups == null || view == null)
                return;

            PropertyGroupDescription groupDescription = new PropertyGroupDescription("Group");

            foreach (var group in Groups)
                groupDescription.GroupNames.Add(group.Name.ToUpper());

            // First clear the existing list
            view.GroupDescriptions.Clear();

            view.GroupDescriptions.Add(groupDescription);
        }
    }
}

namespace NotedUI.Models
{
    public class Group
    {
        public long ID { get; set; }
        public string Name { get; set; }

        public Group()
        { }

        public Group(long id, string groupName)
        {
            ID = id;
            Name = groupName;
        }
    }
}

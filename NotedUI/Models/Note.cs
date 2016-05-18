using System;

namespace NotedUI.Models
{
    public class Note
    {
        public long ID { get; set; }
        public string CloudKey { get; set; }
        public DateTime? LastModified { get; set; }
        public string Content { get; set; }
        public string Group { get; set; }

        public Note()
        { }

        public Note(string content,
                    string group)
        {
            Content = content;
            Group = group;
        }

        public Note(long id,
                    string cloudKey,
                    DateTime? lastModified,
                    string content,
                    string group)
        {
            ID = id;
            CloudKey = cloudKey;
            LastModified = lastModified;
            Content = content;
            Group = group;
        }
    }
}

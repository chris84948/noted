using System;

namespace NotedUI.Models
{
    public class Note
    {
        public long ID { get; set; }
        public string CloudKey { get; set; }
        public DateTime? LastModified { get; set; }
        public string Content { get; set; }
        public string Folder { get; set; }

        public Note()
        { }

        public Note(string content,
                    string folder)
        {
            Content = content;
            Folder = folder;
        }

        public Note(long id,
                    string cloudKey,
                    DateTime? lastModified,
                    string content,
                    string folder)
        {
            ID = id;
            CloudKey = cloudKey;
            LastModified = lastModified;
            Content = content;
            Folder = folder;
        }
    }
}

using System;

namespace NotedUI.Models
{
    public class Note
    {
        public string ID { get; set; }
        public DateTime? LastModified { get; set; }
        public string Content { get; set; }
        public string Folder { get; set; }

        public Note(string id,
                    DateTime? lastModified,
                    string content,
                    string folder)
        {
            this.ID = id;
            this.LastModified = lastModified;
            this.Content = content;
            this.Folder = folder;
        }
    }
}

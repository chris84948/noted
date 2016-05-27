using Newtonsoft.Json;
using System;

namespace NotedUI.Models
{
    public class Note
    {
        public long ID { get; set; }
        public string Content { get; set; }
        public string Group { get; set; }

        // Don't serialize the cloud key or modified date - they're part of the cloud file itself
        [JsonIgnore]
        public string CloudKey { get; set; }
        [JsonIgnore]
        public DateTime? LastModified { get; set; }

        public Note()
        { }

        // This constructor is used when getting all notes from the cloud
        public Note(string cloudKey,
                    DateTime? lastModified)
        {
            CloudKey = cloudKey;
            LastModified = lastModified;
        }

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

        public string ToJSON()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static Note FromJSON(string jsonString)
        {
            return JsonConvert.DeserializeObject<Note>(jsonString);
        }
    }
}

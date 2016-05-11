using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotedUI.Models
{
    public class Folder
    {
        public long ID { get; set; }
        public string Name { get; set; }

        public Folder(long id, string folderName)
        {
            ID = id;
            Name = folderName;
        }
    }
}

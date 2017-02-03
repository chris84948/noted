using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotedUI.Models
{
    public class InstallFile
    {
        public eUpdateType UpdateType { get; set; }
        public string Filename { get; set; }
        public DateTime LastModified { get; set; }
        public string CloudID { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NotedUI.Models
{
    public delegate void FileDoubleClickedRoutedEventHandler(object sender, FileDoubleClickedRoutedEventArgs e);
    public class FileDoubleClickedRoutedEventArgs : RoutedEventArgs
    {
        public string Path { get; set; }

        public FileDoubleClickedRoutedEventArgs(RoutedEvent routedEvent, string path)
            : base(routedEvent)
        {
            Path = path;
        }
    }
}

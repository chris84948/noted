using System.IO;

namespace NotedUI.Export
{
    public class TextExporter
    {
        public static void Export(string filename,
                                  string css,
                                  string content)
        {
            File.WriteAllText(filename, content);

            System.Diagnostics.Process.Start(filename);
        }
    }
}

using Pechkin;
using System.IO;

namespace NotedUI.Export
{
    public class PDFExporter
    {
        public static void Export(string filename,
                                  string cssFilename,
                                  string htmlContent)
        {
            string cssContent = File.ReadAllText(Path.Combine(App.AppPath, $@"Resources\CSS\{ cssFilename }.css"));
            string html = HTMLExporter.CompileHTMLDoc(filename, cssContent, htmlContent);
            
            File.WriteAllBytes(filename, new SimplePechkin(new GlobalConfig()).Convert(html));

            System.Diagnostics.Process.Start(filename);
        }
    }
}

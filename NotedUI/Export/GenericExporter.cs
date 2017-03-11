using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotedUI.Export
{
    class GenericExporter
    {
        public static void Export(string exportType,
                                  string filename,
                                  string cssFilename,
                                  string htmlContent)
        {
            string cssContent = File.ReadAllText($@"Resources\CSS\{ cssFilename }.css");
            string html = HTMLExporter.CompileHTMLDoc(filename, cssContent, htmlContent);

            string tempFilename = Path.GetTempFileName();

            File.WriteAllText(tempFilename, html);

            PandocConverter.Export("html", exportType, tempFilename, filename);

            File.Delete(tempFilename);

            System.Diagnostics.Process.Start(filename);
        }
    }
}

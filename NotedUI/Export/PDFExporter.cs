using System.IO;

namespace NotedUI.Export
{
    public class PDFExporter
    {
        public static void Export(string filename,
                                  string cssFilename,
                                  string htmlContent)
        {
            string cssContent = File.ReadAllText($@"Resources\CSS\{ cssFilename }.css");

            File.WriteAllBytes(filename, 
                               (new NReco.PdfGenerator.HtmlToPdfConverter())
                                    .GeneratePdf(HTMLExporter.CompileHTMLDoc(cssContent, htmlContent)));

            System.Diagnostics.Process.Start(filename);
        }
    }
}

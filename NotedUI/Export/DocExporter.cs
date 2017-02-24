using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using NotesFor.HtmlToOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotedUI.Export
{
    class DocExporter
    {
        public static void Export(string filename,
                                  string cssFilename,
                                  string htmlContent)
        {
            string cssContent = File.ReadAllText($@"Resources\CSS\{ cssFilename }.css");
            string html = HTMLExporter.CompileHTMLDoc(filename, cssContent, htmlContent);

            using (MemoryStream generatedDocument = new MemoryStream())
            {
                using (var package = WordprocessingDocument.Create(generatedDocument, WordprocessingDocumentType.Document))
                {
                    MainDocumentPart mainPart = package?.AddMainDocumentPart();
                    if (mainPart == null)
                        return;

                    new Document(new Body()).Save(mainPart);
                    new HtmlConverter(mainPart).ParseHtml(html);

                    mainPart.Document.Save();
                }

                File.WriteAllBytes(filename, generatedDocument.ToArray());
            }

            System.Diagnostics.Process.Start(filename);
        }
    }
}

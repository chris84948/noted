using System.IO;
using NotedUI.Utilities;

namespace NotedUI.Export
{
    public class HTMLExporter
    {
        private static string GetHtmlHeader(string filename)
        {
            return $@"
<!DOCTYPE html PUBLIC "" -//W3C//DTD HTML 4.01//EN"">
<html>
<head>
  <style type = ""text/css"">";
        }

        private static string htmlPostCss = @"
	</style>
</head>

<body>

";

        private static string htmlFooter = @"

    </body>
</html>";

        public static void Export(string filename,
                                  string cssFilename,
                                  string htmlContent) 
        {
            string cssContent = File.ReadAllText(Path.Combine(App.AppPath, $@"Resources\CSS\{ cssFilename }.css"));

            File.WriteAllText(filename,
                              CompileHTMLDoc(filename, cssContent, htmlContent));

            System.Diagnostics.Process.Start(filename);
        }

        public static string CompileHTMLDoc(string filename, string css, string html)
        {
            return GetHtmlHeader(filename) + css + htmlPostCss + MarkdownParser.Parse(html) + htmlFooter;
        }
    }
}

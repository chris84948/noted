using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotedUI.Export
{
    public class HTMLExporter
    {
        private static string htmlHeader = @"
<!DOCTYPE html PUBLIC "" -//W3C//DTD HTML 4.01//EN"">
<html>
<head>
  <title> My first styled page</title>
  <style type = ""text/css"">";

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
            string cssContent = File.ReadAllText($@"Resources\CSS\{ cssFilename }.css");

            File.WriteAllText(filename,
                              htmlHeader + cssContent + htmlPostCss + htmlContent + htmlFooter);

            System.Diagnostics.Process.Start(filename);
        }

        public static string CompileHTMLDoc(string css, string html)
        {
            return htmlHeader + css + htmlPostCss + html + htmlFooter;
        }
    }
}

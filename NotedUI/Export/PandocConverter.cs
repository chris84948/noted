using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotedUI.Export
{
    class PandocConverter
    {
        public static void Export(string fromExt, string toExt, string filename, string exportFilename)
        {
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = $@"Resources\Pandoc.exe";
            startInfo.Arguments = $@"""{ filename }"" -f { fromExt } -t { toExt } -o ""{ exportFilename }"" -s";
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.StartInfo = startInfo;
            process.Start();

            process.WaitForExit();

            if (process.ExitCode != 0)
                throw new Exception("File export failed to complete successfully.");
        }
    }
}

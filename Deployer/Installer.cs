using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployer
{
    public class Installer
    {
        private const string INNO_INSTALLER = @"C:\Program Files (x86)\Inno Setup 5\ISCC.exe";

        public static void Build(string installDir, Version version)
        {
            CreateInstallerExe(installDir, version);
        }

        private static void CreateInstallerExe(string installDir, Version version)
        {
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = $@"""{ INNO_INSTALLER }""";
            startInfo.Arguments = $@"""/DAppVersion={ version.ToString() }"" ""/DInstallerDirectory={ installDir }"" ""Install.iss""";

            process.StartInfo = startInfo;
            process.Start();

            process.WaitForExit();

            if (process.ExitCode != 0)
                throw new Exception("Inno Installer file creation failed.");

            File.Move(Path.Combine(installDir, "Noted.exe"), Path.Combine(installDir, $"Noted_{ version.ToString() }.exe"));
        }
    }
}

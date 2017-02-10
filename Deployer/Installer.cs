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

            CopyInstallerExeToAllInstallsFolder(installDir, version);
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
        }

        private static void CopyInstallerExeToAllInstallsFolder(string installDir, Version version)
        {
            File.Copy(installDir + "\\Noted.exe", Path.Combine(installDir, "Installs") + $"\\Noted_{ version.ToString() }.exe");
        }
    }
}

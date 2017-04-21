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
            string installExe = Path.Combine(installDir, "Noted.exe");

            if (File.Exists(installExe))
                File.Delete(installExe);

            CreateInstallerExe(installDir, version);
        }

        private static void CreateInstallerExe(string installDir, Version version)
        {
            StringBuilder batch = new StringBuilder();

            batch.Append($@"""{ INNO_INSTALLER }"" ");
            batch.Append($@"""/DAppVersion={ version.ToString() }"" ");
            batch.Append($@"""/DInstallerDirectory={ installDir }"" ");
            batch.Append(@"""Install.iss""");
            batch.AppendLine();
            batch.AppendLine("PING 127.0.0.1 -n 6 > nul");

            string deployBatchFilename = Path.GetTempPath() + "Deploy.bat";
            File.WriteAllText(deployBatchFilename, batch.ToString());

            var process = Process.Start(deployBatchFilename);
            process.WaitForExit();

            File.Delete(deployBatchFilename);

            if (process.ExitCode != 0)
                throw new Exception("Inno Installer file creation failed.");

            if (File.Exists(Path.Combine(installDir, $"Noted_{ version.ToString() }.exe")))
                File.Delete(Path.Combine(installDir, $"Noted_{ version.ToString() }.exe"));

            File.Move(Path.Combine(installDir, "Noted.exe"), Path.Combine(installDir, $"Noted_{ version.ToString() }.exe"));
        }
    }
}

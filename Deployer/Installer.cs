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
            StringBuilder batch = new StringBuilder();

            batch.Append($@"""{ INNO_INSTALLER }"" ");
            batch.Append($@"""/DAppVersion={ version.ToString() }"" ");
            batch.Append($@"""/DInstallerDirectory={ installDir }"" ");
            batch.Append($@"""/DExeFilename={ $"Noted_{ version.ToString() }.exe" }"" ");
            batch.Append(@"""Install.iss""");
            batch.AppendLine();
            batch.AppendLine("PAUSE");

            string deployBatchFilename = Path.GetTempPath() + "Deploy.bat";
            File.WriteAllText(deployBatchFilename, batch.ToString());

            var process = Process.Start(deployBatchFilename);
            process.WaitForExit();

            File.Delete(deployBatchFilename);

            if (process.ExitCode != 0)
                throw new Exception("Inno Installer file creation failed.");
        }
    }
}

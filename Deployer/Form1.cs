using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Deployer
{
    public partial class Form1 : Form
    {
        private const string NOTED_SOLUTION_FOLDER = @"C:\Github\NotedUI";
        private const string DEPLOY_FOLDER = @"..\..\..\build\Squirrel_Builds";
        private const string INSTALLER_FOLDER = @"..\..\..\build\Installers";
        private const string APPX_FOLDER = @"..\..\..\build\AppX";
        private const string NUGET_PATH = @"C:\Google Drive\Utilities\Nuget.exe";
        private const string SQUIRREL_PATH = @"C:\Google Drive\Utilities\Squirrel.Windows\Squirrel.exe";

        public Form1()
        {
            InitializeComponent();
            
            tbVersion.Text = Properties.Settings.Default.LastVersion;
        }
        
        private void buttonDeploy_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.LastVersion = tbVersion.Text;
            Properties.Settings.Default.Save();

            Version version = GetVersion();

            if (version == null)
                return;

            AppBuild.BuildRelease(version);

            BuildNugetAndSquirrelPackage(version);
            
            Application.Exit();
        }

        private void buttonInstaller_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.LastVersion = tbVersion.Text;
            Properties.Settings.Default.Save();

            Version version = GetVersion();

            if (version == null)
                return;

            AppBuild.BuildRelease(version);

            Installer.Build(INSTALLER_FOLDER, version);

            BuildAppX(version);            

            Application.Exit();
        }

        private Version GetVersion()
        {
            try
            {
                return new Version(tbVersion.Text);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private void BuildNugetAndSquirrelPackage(Version version)
        {
            StringBuilder batch = new StringBuilder();
            string versionString = version.Revision == 0 ? $"{ version.Major }.{ version.Minor }.{ version.Build }" : version.ToString();

            batch.Append($@"""{ NUGET_PATH }"" ");
            batch.Append($@"pack ""{ NOTED_SOLUTION_FOLDER }\NotedUI\NotedUI.nuspec"" ");
            batch.Append($@"-Prop Configuration=Release ");
            batch.Append($@"-Version { version.ToString() } ");
            batch.Append($@"-OutputDirectory ""{ DEPLOY_FOLDER }""");

            batch.AppendLine();
            batch.Append($@"""{ SQUIRREL_PATH }"" ");
            batch.Append($@"--releasify ""{ DEPLOY_FOLDER }\noted.{ versionString }.nupkg"" ");
            batch.Append($@"--releaseDir=""{ DEPLOY_FOLDER }\Releases""");

            batch.AppendLine();
            batch.AppendLine("PING 127.0.0.1 -n 6 > nul");

            string deployBatchFilename = Path.GetTempPath() + "Deploy.bat";
            File.WriteAllText(deployBatchFilename, batch.ToString());

            var process = Process.Start(deployBatchFilename);
            process.WaitForExit();

            File.Delete(deployBatchFilename);

            if (process.ExitCode != 0)
                throw new Exception("Nuget & Squirrel packaging failed.");

            if (File.Exists($@"{ DEPLOY_FOLDER }\Releases\Setup.msi"))
                File.Delete($@"{ DEPLOY_FOLDER }\Releases\Setup.msi");
        }

        private void BuildAppX(Version version)
        {
            if (Directory.Exists(Path.Combine(INSTALLER_FOLDER, "Noted_" + version.ToString())))
                Directory.Delete(Path.Combine(INSTALLER_FOLDER, "Noted_" + version.ToString()), true);

            ProcessStartInfo proc = new ProcessStartInfo();
            proc.UseShellExecute = true;
            proc.FileName = Path.Combine(APPX_FOLDER, "MakeAppX.bat");
            proc.Arguments = version.ToString();
            proc.Verb = "runas";

            Process.Start(proc);
        }
    }
}
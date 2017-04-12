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
        private const string DEPLOY_FOLDER = @"C:\Google Drive\Programming\Noted\Nuget_Builds";
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
    }
}
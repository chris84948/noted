using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
        private const string DEPLOY_FOLDER = @"C:\NotedTest";

        public Form1()
        {
            InitializeComponent();

            // HACK do this for now
            tbVersion.Text = "0.1.0.1";
        }
        
        private void buttonDeploy_Click(object sender, EventArgs e)
        {
            Version version = GetVersion();

            if (version == null)
                return;

            AppBuild.BuildRelease(version);

            Installer.Build(DEPLOY_FOLDER, version);

            CopyReleaseFilesToLatestFolder(Path.Combine(DEPLOY_FOLDER, "Latest"));
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

        private void CopyReleaseFilesToLatestFolder(string latestReleaseFolder)
        {
            // First delete any old files
            foreach (var file in new DirectoryInfo(latestReleaseFolder).GetFiles())
                File.Delete(file.FullName);

            string sourcePath = Path.Combine(NOTED_SOLUTION_FOLDER, "NotedUI", "Bin", "Release");
            string destinationPath = Path.Combine(DEPLOY_FOLDER, "Latest");
            var excludeTypes = new List<string>() { "db" };
            
            //Now Create all of the directories
            foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
                Directory.CreateDirectory(dirPath.Replace(sourcePath, destinationPath));

            //Copy all the files & Replaces any files with the same name
            foreach (string newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
                File.Copy(newPath, newPath.Replace(sourcePath, destinationPath), true);
        }
    }
}

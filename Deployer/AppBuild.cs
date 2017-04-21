using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployer
{
    class AppBuild
    {
        private const string MSBUILD_LOCATION = @"C:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild";

        public static void BuildRelease(Version version, bool demoVersion = false)
        {
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = $@"""{ MSBUILD_LOCATION }""";
            startInfo.Arguments = $@"""{ @"C:\GitHub\notedui\NotedUI\NotedUI.csproj" }"" /target:clean,rebuild ";
            startInfo.Arguments += $@"/p:Configuration=Release;PlatformTarget=x86;";
            startInfo.Arguments += $@"VersionNumber={ version.ToString() };ApplicationVersion={ version.ToString() }";
			
			//if (demoVersion)
			//	startInfo.Arguments += $@";DefineConstants=TRIAL";

            process.StartInfo = startInfo;
            process.Start();

            process.WaitForExit();

            if (process.ExitCode != 0)
                throw new Exception("Project build failed.");
        }
    }
}

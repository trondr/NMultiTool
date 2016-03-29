using System;
using System.Collections;
using System.ComponentModel;
using System.IO;

namespace NMultiTool.Tests
{
    [RunInstaller(true)]
    public partial class InstallerExample : System.Configuration.Install.Installer
    {
        public static string InstalledFile = Path.Combine(Environment.ExpandEnvironmentVariables("%TEMP%"), "NMultiTool.Tests.InstallerExample.Installed.txt");
        public static string UnInstalledFile = Path.Combine(Environment.ExpandEnvironmentVariables("%TEMP%"), "NMultiTool.Tests.InstallerExample.UnInstalled.txt");

        public InstallerExample()
        {
            InitializeComponent();            
        }

        public override void Install(IDictionary stateSaver)
        {
            this.Context.LogMessage("Installing NMultiTool.Library.Tests.InstallerExample...");

            using (var sw = File.CreateText(InstalledFile))
            {
                sw.WriteLine("Installed");
            }
            if(File.Exists(UnInstalledFile)) File.Delete(UnInstalledFile);

            base.Install(stateSaver);
            this.Context.LogMessage("Finished Installing NMultiTool.Library.Tests.InstallerExample.");
        }

        public override void Uninstall(IDictionary savedState)
        {
            this.Context.LogMessage("UnInstalling NMultiTool.Library.Tests.InstallerExample...");
            using (var sw = File.CreateText(UnInstalledFile))
            {
                sw.WriteLine("UnInstalled");
            }
            if (File.Exists(InstalledFile)) File.Delete(InstalledFile);
            base.Uninstall(savedState);
            this.Context.LogMessage("Finished UnInstalling NMultiTool.Library.Tests.InstallerExample.");
        }
    }
}

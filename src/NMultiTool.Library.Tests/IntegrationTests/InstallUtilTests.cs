using System;
using System.Collections.Generic;
using System.IO;
using Common.Logging;
using Common.Logging.Simple;
using NMultiTool.Library.BootStrap;
using NMultiTool.Library.Commands.InstallUtil;
using NUnit.Framework;
using Rhino.Mocks;

namespace NMultiTool.Library.Tests.IntegrationTests
{
    [TestFixture(Category = "IntegrationTests")]
    public class InstallUtilTests
    {
        private ILog _logger;
        private IFolderFileSearcher _folderFileSearcher;
        private ILoggingConfiguration _stubLoggingConfiguration;
        private string _setupLogDirectory;
        private FileInfo _assemblyFileInfo;
        private List<string> _includeFileSpecification;
        private List<string> _excludeFileSpecifications;

        [SetUp]
        public void SetUp()
        {
            _logger = new ConsoleOutLogger("InstallUtilTests", LogLevel.All, true, false, false, "yyyy-MM-dd hh:mm:ss");
            _folderFileSearcher = new FolderFileSearcher();
            _setupLogDirectory = Path.Combine(Environment.ExpandEnvironmentVariables("%TEMP%"), "NMultiToolInstallUtilIntegrationTestsSetupLogs");
            if (!Directory.Exists(_setupLogDirectory)) Directory.CreateDirectory(_setupLogDirectory);            
            _stubLoggingConfiguration = MockRepository.GenerateStub<ILoggingConfiguration>();
            _stubLoggingConfiguration.LogDirectoryPath = _setupLogDirectory;
            _assemblyFileInfo = new FileInfo(typeof(InstallerExample).Assembly.Location);
            _includeFileSpecification = new List<string>() {_assemblyFileInfo.Name};
            _excludeFileSpecifications = new List<string>();
            if (File.Exists(InstallerExample.InstalledFile)) File.Delete(InstallerExample.InstalledFile);
            if (File.Exists(InstallerExample.UnInstalledFile)) File.Delete(InstallerExample.UnInstalledFile);
        }
        
        [TearDown]
        public void TearDown()
        {
            if (Directory.Exists(_setupLogDirectory)) Directory.Delete(_setupLogDirectory, true);
        }

        [Test]
        public void InstallUtilTest()
        {
            Console.WriteLine("Integration testing InstallUtil by installing current test assembly '{0}' which contains installer class '{1}'",typeof(InstallUtilTests).Assembly.Location, typeof(InstallerExample).FullName);
            var target = new InstallUtil(_stubLoggingConfiguration, _folderFileSearcher, _logger);
            if (_assemblyFileInfo.Directory == null) throw new ArgumentNullException();
            Assert.AreEqual(1, _folderFileSearcher.GetFiles(_assemblyFileInfo.Directory.FullName,_includeFileSpecification).Count, "Number of assemblies to be processed by InstallUtil should be 1.");
            Assert.AreEqual(0, _folderFileSearcher.GetFiles(_assemblyFileInfo.Directory.FullName, _excludeFileSpecifications).Count, "Number of assemblies to be excluded by InstallUtil should be 0.");

            Assert.IsFalse(File.Exists(InstallerExample.InstalledFile), "'Installed' exists when it should not: " + InstallerExample.InstalledFile);
            Assert.IsFalse(File.Exists(InstallerExample.UnInstalledFile), "'UnInstalled' exists when it should not: " + InstallerExample.UnInstalledFile);
            
            target.Execute(InstallAction.Install, _assemblyFileInfo.Directory.FullName, _includeFileSpecification, _excludeFileSpecifications);
            Assert.IsTrue(File.Exists(InstallerExample.InstalledFile),"'Installed' file does not exist: " + InstallerExample.InstalledFile);
            Assert.IsFalse(File.Exists(InstallerExample.UnInstalledFile), "'UnInstalled' exists when it should not: " + InstallerExample.UnInstalledFile);
            
            target.Execute(InstallAction.UnInstall, _assemblyFileInfo.Directory.FullName, _includeFileSpecification, _excludeFileSpecifications);
            Assert.IsTrue(File.Exists(InstallerExample.UnInstalledFile), "'UnInstalled' file does not exist: " + InstallerExample.UnInstalledFile);
            Assert.IsFalse(File.Exists(InstallerExample.InstalledFile), "'Installed' exists when it should not: " + InstallerExample.InstalledFile);

            target.Execute(InstallAction.UnInstallInstall, _assemblyFileInfo.Directory.FullName, _includeFileSpecification, _excludeFileSpecifications);
            Assert.IsTrue(File.Exists(InstallerExample.InstalledFile), "'Installed' file does not exist: " + InstallerExample.InstalledFile);
            Assert.IsFalse(File.Exists(InstallerExample.UnInstalledFile), "'UnInstalled' exists when it should not: " + InstallerExample.UnInstalledFile);
            
            target.Execute(InstallAction.UnInstall, _assemblyFileInfo.Directory.FullName, _includeFileSpecification, _excludeFileSpecifications);
            Assert.IsTrue(File.Exists(InstallerExample.UnInstalledFile), "'UnInstalled' file does not exist: " + InstallerExample.UnInstalledFile);
            Assert.IsFalse(File.Exists(InstallerExample.InstalledFile), "'Installed' exists when it should not: " + InstallerExample.InstalledFile);
        }
    }
}

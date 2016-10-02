using System.Collections.Generic;
using System.IO;
using NMultiTool.Library.Module.Commands.ConvertSvgToIco;

namespace NMultiTool.Library.Module.Commands.BackupGitRepositories
{
    public class BackupGitRepositoriesConfigurationProvider : IBackupGitRepositoriesConfigurationProvider
    {
        private readonly IConfigurationProvider<BackupGitRepositoriesConfiguration> _configurationProvider;

        public BackupGitRepositoriesConfigurationProvider(IConfigurationProvider<BackupGitRepositoriesConfiguration> configurationProvider)
        {
            _configurationProvider = configurationProvider;
        }
        
        public BackupGitRepositoriesConfiguration Load(string configurationFile)
        {
            VerifyConfigurationFileOrCreateExampleConfigurationFile(configurationFile);
            var configuration = _configurationProvider.Load(configurationFile);
            return configuration;
        }

        public void Save(BackupGitRepositoriesConfiguration configuration, string configurationFile)
        {
            _configurationProvider.Save(configuration,configurationFile);
        }

        private void VerifyConfigurationFileOrCreateExampleConfigurationFile(string configurationFile)
        {
            if (!File.Exists(configurationFile))
            {
                var exampleConfigurationFile = configurationFile + ".example";
                var exampleConfiguration = new BackupGitRepositoriesConfiguration()
                {
                    Repositories = new List<GitRepository>()
                    {
                        new GitRepository {SourceUrl = "http://github.com/someowner/somerepository1.git"},
                        new GitRepository {SourceUrl = "http://github.com/someowner/somerepository2.git"}
                    },
                    ReportMail = "firstname.lastname@somecompany.com",
                    RootBackupFolder = @"c:\Backup\GitRepositories"
                };
                Save(exampleConfiguration,exampleConfigurationFile);
                throw new NMultiToolException("Configuration file does not exist. An example configuration file has been saved to: " + exampleConfigurationFile);
            }
        }
    }
}
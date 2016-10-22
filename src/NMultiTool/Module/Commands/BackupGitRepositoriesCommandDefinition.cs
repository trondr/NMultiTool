using NCmdLiner.Attributes;
using NMultiTool.Library.Infrastructure;
using NMultiTool.Library.Module.Commands.BackupGitRepositories;

namespace NMultiTool.Module.Commands
{
    public class BackupGitRepositoriesCommandDefinition: CommandDefinition
    {
        private readonly IBackupGitRepositoriesCommandProvider _backupGitRepositoriesCommandProvider;

        public BackupGitRepositoriesCommandDefinition(IBackupGitRepositoriesCommandProvider backupGitRepositoriesCommandProvider)
        {
            _backupGitRepositoriesCommandProvider = backupGitRepositoriesCommandProvider;            
        }


        [Command(Description = "Backup git repositories.")]
        public int BackupGitRepositories(
            [RequiredCommandParameter(Description = "Path to configuration file", AlternativeName = "cf", ExampleValue = @"c:\temp\BackupGitRepositories.config")]
            string configurationFile)
        {
            return _backupGitRepositoriesCommandProvider.BackupGitRepositories(configurationFile);
        }
    }
}

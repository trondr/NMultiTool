namespace NMultiTool.Library.Module.Commands.BackupGitRepositories
{
    public interface IBackupGitRepositoriesConfigurationProvider
    {
        BackupGitRepositoriesConfiguration Load(string configurationFile);

        void Save(BackupGitRepositoriesConfiguration configuration, string configurationFile);
    }
}
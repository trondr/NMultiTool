namespace NMultiTool.Library.Module.Commands.BackupGitRepositories
{
    public interface IBackupGitRepositoriesConfigurationProvider
    {
        BackupGitRepositoriesConfiguration Load(string configurationFile);
    }

    public class BackupGitRepositoriesConfigurationProvider : IBackupGitRepositoriesConfigurationProvider
    {
        public BackupGitRepositoriesConfiguration Load(string configurationFile)
        {
            throw new System.NotImplementedException();
        }
    }
}
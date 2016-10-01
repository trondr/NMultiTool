namespace NMultiTool.Library.Module.Commands.BackupGitRepositories
{
    public interface IGitRepositoryBackupFolderProvider
    {
        string GetRepositoryBackupFolder(string rootBackupFolder, string sourceUrl);
    }
}
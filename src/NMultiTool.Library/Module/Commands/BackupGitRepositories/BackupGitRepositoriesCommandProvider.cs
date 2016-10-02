using System.IO;
using Common.Logging;
using NMultiTool.Library.Module.Common.Process;

namespace NMultiTool.Library.Module.Commands.BackupGitRepositories
{
    public class BackupGitRepositoriesCommandProvider : IBackupGitRepositoriesCommandProvider
    {
        private readonly IBackupGitRepositoriesConfigurationProvider _backupGitRepositoriesConfigurationProvider;
        private readonly IGitRepositoryBackupFolderProvider _gitRepositoryBackupFolderProvider;
        private readonly IProcess _process;
        private readonly ILog _logger;

        public BackupGitRepositoriesCommandProvider(IBackupGitRepositoriesConfigurationProvider backupGitRepositoriesConfigurationProvider, IGitRepositoryBackupFolderProvider gitRepositoryBackupFolderProvider, IProcess process, ILog logger)
        {
            _backupGitRepositoriesConfigurationProvider = backupGitRepositoriesConfigurationProvider;
            _gitRepositoryBackupFolderProvider = gitRepositoryBackupFolderProvider;
            _process = process;
            _logger = logger;
        }

        public int BackupGitRepositories(string backupGitRepositoriesConfigurationFile)
        {
            var backupGitRepositoriesConfiguration = _backupGitRepositoriesConfigurationProvider.Load(backupGitRepositoriesConfigurationFile);
            var rootBackupFolder = GetAndValidateRootBackupFolder(backupGitRepositoriesConfiguration);

            var gitRepositories = backupGitRepositoriesConfiguration.Repositories;
            foreach (var gitRepository in gitRepositories)
            {
                var sourceUrl = gitRepository.SourceUrl;
                var localBackupRepositoryFolder = _gitRepositoryBackupFolderProvider.GetRepositoryBackupFolder(rootBackupFolder, gitRepository.SourceUrl);
                if (!Directory.Exists(localBackupRepositoryFolder))
                {
                    CloneRepository(sourceUrl, localBackupRepositoryFolder);
                }
                BackupRepository(localBackupRepositoryFolder);
            }
            return 0;
        }

        private void BackupRepository(string localBackupRepositoryFolder)
        {
            using (new ChangeCurrentDirectory(localBackupRepositoryFolder))
            {
                _process.Reset();
                _process.Execute("git.exe", "pull", true, localBackupRepositoryFolder);
                _logger.Info(_process.StandardOutput);
            }
        }

        private void CloneRepository(string sourceUrl, string localBackupRepositoryFolder)
        {
            if (!Directory.Exists(localBackupRepositoryFolder))
            {
                Directory.CreateDirectory(localBackupRepositoryFolder);
            }
            _process.Reset();
            var arguments = string.Format("clone \"{0}\" \"{1}\"", sourceUrl, localBackupRepositoryFolder);
            _process.Execute("git.exe", arguments, true, localBackupRepositoryFolder);
            _logger.Info(_process.StandardOutput);
        }

        private string GetAndValidateRootBackupFolder(BackupGitRepositoriesConfiguration backupGitRepositoriesConfiguration)
        {
            var rootBackupFolder = backupGitRepositoriesConfiguration.RootBackupFolder;
            if (!Directory.Exists(rootBackupFolder))
            {
                var message = string.Format("Root backup folder '{0}' not found. Please create this directory manually and try again.", rootBackupFolder);
                throw new DirectoryNotFoundException(message);
            }
            return rootBackupFolder;
        }
    }
}
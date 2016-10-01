using System;
using System.IO;

namespace NMultiTool.Library.Module.Commands.BackupGitRepositories
{
    public class BackupGitRepositoriesCommandProvider : IBackupGitRepositoriesCommandProvider
    {
        private readonly IBackupGitRepositoriesConfigurationProvider _backupGitRepositoriesConfigurationProvider;
        private readonly IGitRepositoryBackupFolderProvider _gitRepositoryBackupFolderProvider;

        public BackupGitRepositoriesCommandProvider(IBackupGitRepositoriesConfigurationProvider backupGitRepositoriesConfigurationProvider, IGitRepositoryBackupFolderProvider gitRepositoryBackupFolderProvider)
        {
            _backupGitRepositoriesConfigurationProvider = backupGitRepositoriesConfigurationProvider;
            _gitRepositoryBackupFolderProvider = gitRepositoryBackupFolderProvider;
        }

        public void BackupGitRepositories(string backupGitRepositoriesConfigurationFile)
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
        }

        private void BackupRepository(string localBackupRepositoryFolder)
        {
            using (new ChangeCurrentDirectory(localBackupRepositoryFolder))
            {
                //git pull -V    
            }            
            throw new NotImplementedException();
        }

        private void CloneRepository(string sourceUrl, string localBackupRepositoryFolder)
        {
            if (!Directory.Exists(localBackupRepositoryFolder))
            {
                Directory.CreateDirectory(localBackupRepositoryFolder);
            }
            //git clone "<sourceUrl>" "<localBackupRepositoryFolder>"
            throw new NotImplementedException();
        }

        private string GetAndValidateRootBackupFolder(BackupGitRepositoriesConfiguration backupGitRepositoriesConfiguration)
        {
            var rootBackupFolder = backupGitRepositoriesConfiguration.RootBackupFolder;
            if (!File.Exists(rootBackupFolder))
            {
                var message = string.Format("Root backup folder '{0}' not found. Please create this directory manually and try again.", rootBackupFolder);
                throw new DirectoryNotFoundException(message);
            }
            return rootBackupFolder;
        }
    }
}
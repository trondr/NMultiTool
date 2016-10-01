using System.Collections.Generic;

namespace NMultiTool.Library.Module.Commands.BackupGitRepositories
{
    public class BackupGitRepositoriesConfiguration
    {
        public List<GitRepository> Repositories { get; set; }

        public string ReportMail { get; set; }

        public string RootBackupFolder { get; set; }
    }
}
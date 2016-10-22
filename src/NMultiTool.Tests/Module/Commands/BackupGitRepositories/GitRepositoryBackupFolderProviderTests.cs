using NMultiTool.Library.Module.Commands.BackupGitRepositories;
using NUnit.Framework;

namespace NMultiTool.Tests.Module.Commands.BackupGitRepositories
{
    [TestFixture()]
    public class GitRepositoryBackupFolderProviderTests
    {
        [Test]
        [TestCase("https://github.com/someuser/some-repository.git",@"c:\temp\rootBackupFolder", @"c:\temp\rootBackupFolder\github-com-someuser\some-repository")]
        [TestCase("https://someservices.kilnhg.com/Code/Repositories/Legacy/some-repository.git", @"c:\temp\rootBackupFolder", @"c:\temp\rootBackupFolder\someservices-kilnhg-com-code-repositories-legacy\some-repository")]
        [TestCase("https://someservices.kilnhg.com/Code/Repositories/Tools/some-repository.git", @"c:\temp\rootBackupFolder", @"c:\temp\rootBackupFolder\someservices-kilnhg-com-code-repositories-tools\some-repository")]  
        [TestCase("https://someuser@bitbucket.org/some-team/some-repository.git", @"c:\temp\rootBackupFolder", @"c:\temp\rootBackupFolder\bitbucket-org-some-team\some-repository")]
        [TestCase("file://c:\\temp\\some-repository", @"c:\temp\rootBackupFolder", @"c:\temp\rootBackupFolder\c-temp\some-repository")]
        [TestCase("file://\\\\someserver\\temp\\some-repository", @"c:\temp\rootBackupFolder", @"c:\temp\rootBackupFolder\someserver-temp\some-repository")]
        public void GetRepositoryBackupFolderTest(string sourceUrl, string rootBackupfolder, string expected)
        {
            var target = new GitRepositoryBackupFolderProvider();
            var actual = target.GetRepositoryBackupFolder(rootBackupfolder, sourceUrl);
            Assert.AreEqual(expected, actual);
        }
    }
}
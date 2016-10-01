using System;
using System.IO;
using System.Linq;

namespace NMultiTool.Library.Module.Commands.BackupGitRepositories
{
    public class GitRepositoryBackupFolderProvider : IGitRepositoryBackupFolderProvider
    {
        public string GetRepositoryBackupFolder(string rootBackupFolder, string sourceUrl)
        {
            var sourceUri = new Uri(sourceUrl);
            var sourceHost = GetSourceHost(sourceUri.Host);
            var sourceOwner = GetRepositoryOwner(sourceUri);
            var repositoryName = GetRepositoryName(sourceUri);
            var sourceRoot = string.Format("{0}-{1}", sourceHost, sourceOwner);
            var repositoryBackupFolder = Path.Combine(rootBackupFolder, sourceRoot, repositoryName);
            return repositoryBackupFolder;
        }

        private string GetSourceHost(string sourceUriHost)
        {
            var sourceHost = sourceUriHost.Replace(".","-");
            return sourceHost;
        }

        private string GetRepositoryOwner(Uri sourceUri)
        {
            var absolutePath = sourceUri.AbsolutePath.TrimStart(new []{'/'});
            var pathParts = absolutePath.ToLower().Split('/');
            var allpathPartsButTheLast = pathParts.Take(pathParts.Length - 1);
            var respositoryOwner = string.Join("-", allpathPartsButTheLast);
            return respositoryOwner;
        }

        private string GetRepositoryName(Uri sourceUri)
        {
            var absolutePath = sourceUri.AbsolutePath.TrimStart(new []{'/'});
            var pathParts = absolutePath.Split('/');
            var respositoryName = pathParts[pathParts.Length - 1];
            respositoryName = Path.GetFileNameWithoutExtension(respositoryName);
            return respositoryName;
        }
    }
}
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
            var sourceHost = GetSourceHost(sourceUri);
            var sourceOwner = GetRepositoryOwner(sourceUri);
            var repositoryName = GetRepositoryName(sourceUri);
            var sourceRoot = string.Format("{0}-{1}", sourceHost, sourceOwner);
            var repositoryBackupFolder = Path.Combine(rootBackupFolder, sourceRoot, repositoryName);
            return repositoryBackupFolder;
        }

        private string GetSourceHost(Uri sourceUri)
        {
            if (sourceUri == null) throw new ArgumentNullException(nameof(sourceUri));

            string sourceHost;
            if (sourceUri.Scheme == "file")
            {
                sourceHost = GetSourceHostFromFileUri(sourceUri);                
            }
            else
            {
                var sourceUriHost = sourceUri.Host;
                sourceHost = sourceUriHost.Replace(".","-");
            }            
            return sourceHost;
        }

        private string GetSourceHostFromFileUri(Uri sourceUri)
        {
            string sourceHost;
            if (sourceUri.IsUnc)
            {
                sourceHost = sourceUri.Host;                
            }
            else
            {
                var driveLetter = sourceUri.LocalPath[0].ToString();
                sourceHost = driveLetter;    
            }            
            return sourceHost;
        }

        private string GetRepositoryOwner(Uri sourceUri)
        {
            string respositoryOwner;
            if (sourceUri.Scheme == "file")
            {
                respositoryOwner = GetOwnerFromFileUri(sourceUri);             
            }
            else
            {
                respositoryOwner = GetOwnerFromWebUri(sourceUri);
            }            
            return respositoryOwner;
        }

        private string GetOwnerFromWebUri(Uri sourceUri)
        {
            var absolutePath = sourceUri.AbsolutePath.TrimStart(new []{'/'});
            var pathParts = absolutePath.ToLower().Split('/');
            var allpathPartsButTheLast = pathParts.Take(pathParts.Length - 1);
            var respositoryOwner = string.Join("-", allpathPartsButTheLast);
            return respositoryOwner;
        }

        private string GetOwnerFromFileUri(Uri sourceUri)
        {
            var absolutePath = sourceUri.AbsolutePath.TrimStart(new[] { '/' });
            var pathParts = absolutePath.ToLower().Split('/');
            var allpathPartsButTheLast = pathParts.Take(pathParts.Length - 1);
            string respositoryOwner;
            if (sourceUri.IsUnc)
            {            
                respositoryOwner = string.Join("-", allpathPartsButTheLast);
            }
            else
            {
                var allpathPartsButTheFirstAndTheLast = allpathPartsButTheLast.ToList();
                allpathPartsButTheFirstAndTheLast.RemoveAt(0);    
                respositoryOwner = string.Join("-", allpathPartsButTheFirstAndTheLast);
            }
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
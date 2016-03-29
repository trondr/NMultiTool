using System;
using System.Collections.Generic;
using System.IO;

namespace NMultiTool.Library.Module.Commands.InstallUtil
{
    public class FolderFileSearcher : IFolderFileSearcher
    {
        public IDictionary<string, FileInfo> GetFiles(string directory, IList<string> fileSpesifications)
        {
            if(!Directory.Exists(directory)) throw new DirectoryNotFoundException("Directory not found:" + directory);
            if(fileSpesifications == null) throw new ArgumentNullException("fileSpesifications");
            var directoryInfo = new DirectoryInfo(directory);
            var files = new Dictionary<string, FileInfo>();
            foreach (var fileSpecification in fileSpesifications)
            {
                foreach (var fileInfo in directoryInfo.GetFiles(fileSpecification))
                {
                    if (!files.ContainsKey(fileInfo.Name))
                    {
                        files.Add(fileInfo.Name, fileInfo);
                    }
                }
            }
            return files;
        }
    }
}
using System.Collections.Generic;
using System.IO;

namespace NMultiTool.Library.Module.Commands.InstallUtil
{
    public interface IFolderFileSearcher
    {
        IDictionary<string, FileInfo> GetFiles(string directory, IList<string> fileSpesifications);
    }
}
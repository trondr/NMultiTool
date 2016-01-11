using System.Security.Cryptography.X509Certificates;

namespace NMultiTool.Library.Commands.Folder2Wxs
{
    public interface IFolder2Wxs
    {
        void Harvest(string path, string wsxFileName, string targetFolderId, string componentGroupId, string[] diskIds, bool addExecutables2AppsPath);
    }
}

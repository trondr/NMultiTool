namespace NMultiTool.Library.Module.Commands.Folder2Wxs
{
    public interface IFolder2Wxs
    {
        void Harvest(string path, string wsxFileName, string targetFolderId, string componentGroupId, string[] diskIds, bool addExecutables2AppsPath);
    }
}

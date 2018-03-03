using NCmdLiner;

namespace NMultiTool.Library.Module.Commands.Folder2Wxs
{
    public interface IFolder2WxsCommandProvider
    {
        Result<int> Folder2Wxs(HarvestInfo harvestInfo);
    }
}
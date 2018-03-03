using NCmdLiner;

namespace NMultiTool.Library.Module.Commands.SplitIco
{
    public interface ISplitIcoCommandProvider
    {
        Result<int> SplitIco(string icoFileName);
    }
}

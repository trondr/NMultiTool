namespace NMultiTool.Library.Module.Commands.SplitIco
{
    public interface ISplitIcoCommandProviderFactory
    {
        ISplitIcoCommandProvider GetSplitIcoCommandProvider();

        void Release(ISplitIcoCommandProvider splitIcoCommandProvider);
    }
}
namespace NMultiTool.Library.Module.Commands.Folder2Wxs
{
    public interface IFolder2WxsCommandProviderFactory
    {
        IFolder2WxsCommandProvider GetFolder2WxsCommandProvider();

        void Release(IFolder2WxsCommandProvider folder2WxsCommandProvider);
    }
}
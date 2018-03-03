namespace NMultiTool.Library.Module.Commands.InstallUtil
{
    public interface IInstallUtilCommandProviderFactory
    {
        IInstallUtilCommandProvider GetInstallUtilCommandProvider();
        void Release(IInstallUtilCommandProvider installUtilCommandProvider);
    }
}
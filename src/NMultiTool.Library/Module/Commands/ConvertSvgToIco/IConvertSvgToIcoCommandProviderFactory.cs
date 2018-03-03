namespace NMultiTool.Library.Module.Commands.ConvertSvgToIco
{
    public interface IConvertSvgToIcoCommandProviderFactory
    {
        IConvertSvgToIcoCommandProvider GetConvertSvgToIcoCommandProvider();

        void Release(IConvertSvgToIcoCommandProvider convertSvgToIcoCommandProvider);
    }
}
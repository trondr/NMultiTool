namespace NMultiTool.Library.Module.Commands.ConvertAllSvgToIco
{
    public interface IConvertAllSvgToIcoCommandProviderFactory
    {
        IConvertAllSvgToIcoCommandProvider GetConvertAllSvgToIcoCommandProvider();

        void Release(IConvertAllSvgToIcoCommandProvider convertAllSvgToIcoCommandProvider);
    }
}
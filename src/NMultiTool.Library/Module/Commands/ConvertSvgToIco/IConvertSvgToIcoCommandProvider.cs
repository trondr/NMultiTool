namespace NMultiTool.Library.Module.Commands.ConvertSvgToIco
{
    public interface IConvertSvgToIcoCommandProvider
    {
        int ConvertSvgToIco(string svgFileName, int[] sizes, bool refresh);
    }
}
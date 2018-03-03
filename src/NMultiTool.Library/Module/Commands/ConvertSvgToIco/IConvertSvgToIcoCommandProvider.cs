using NCmdLiner;

namespace NMultiTool.Library.Module.Commands.ConvertSvgToIco
{
    public interface IConvertSvgToIcoCommandProvider
    {
        Result<int> ConvertSvgToIco(string svgFileName, int[] sizes, bool refresh);
    }
}
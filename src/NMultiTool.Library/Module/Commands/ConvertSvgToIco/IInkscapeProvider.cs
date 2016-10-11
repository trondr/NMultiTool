using NMultiTool.Library.Module.Commands.ConvertAllSvgToIco;

namespace NMultiTool.Library.Module.Commands.ConvertSvgToIco
{
    public interface IInkscapeProvider
    {
        void ExportSvgToPng(IconFileInfo svgFile, PngFileInfo pngFile);

        void ExportSvgToPngs(IconInfo iconInfo);
    }
}
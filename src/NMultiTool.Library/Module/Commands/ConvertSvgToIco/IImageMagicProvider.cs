using NMultiTool.Library.Module.Commands.ConvertAllSvgToIco;

namespace NMultiTool.Library.Module.Commands.ConvertSvgToIco
{
    public interface IImageMagicProvider
    {
        void ResizePng(PngFileInfo sourcePngFile, PngFileInfo targetPngFile);

        void CreateIconFromPngFilesResized(IconInfo iconInfo);

        void CreateIconFromPngFilesFromSvg(IconInfo iconInfo);
    }
}
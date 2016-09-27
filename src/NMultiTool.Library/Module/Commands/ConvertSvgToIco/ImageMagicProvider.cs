using System.IO;
using Common.Logging;
using ImageMagick;
using NMultiTool.Library.Module.Commands.ConvertAllSvgToIco;

namespace NMultiTool.Library.Module.Commands.ConvertSvgToIco
{
    public class ImageMagicProvider : IImageMagicProvider
    {
        private readonly ILog _logger;

        public ImageMagicProvider(ILog logger)
        {
            _logger = logger;
        }

        public void ResizePng(PngFileInfo sourcePngFile, PngFileInfo targetPngFile)
        {
            _logger.InfoFormat("Resizing: {0} -> {1}", sourcePngFile.Name, targetPngFile.Name);
            var sourcePngFileInfo = new FileInfo(sourcePngFile.FullName);
            using (var sourceImage = new MagickImage(sourcePngFileInfo))
            {
                sourceImage.Resize(targetPngFile.Size, targetPngFile.Size);
                var targetPngFileInfo = new FileInfo(targetPngFile.FullName);
                sourceImage.Write(targetPngFileInfo);
            }
        }

        public void CreateIconFromPngFiles(IconInfo iconInfo)
        {
            _logger.InfoFormat("Creating: {0}", iconInfo.IconFile.FullName);
            var pngFileInfo = new FileInfo(iconInfo.LargestPngFile.FullName);
            using (var image = new MagickImage(pngFileInfo))
            {
                var defines = new IconDefines(iconInfo.Sizes);
                var icoFileInfo = new FileInfo(iconInfo.IconFile.FullName);
                image.Write(icoFileInfo,defines);
            }                        
        }
    }
}
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Common.Logging;
using NMultiTool.Library.Module.Commands.ConvertAllSvgToIco;

namespace NMultiTool.Library.Module.Commands.ConvertSvgToIco
{
    public class ConvertSvgToIcoCommandProvider : IConvertSvgToIcoCommandProvider
    {
        private readonly IInkscapeProvider _inkscapeProvider;
        private readonly IImageMagicProvider _imageMagicProvider;
        private readonly IProcessProvider _processProvider;

        private readonly ILog _logger;

        public ConvertSvgToIcoCommandProvider(IInkscapeProvider inkscapeProvider, IImageMagicProvider imageMagicProvider, IProcessProvider processProvider, ILog logger)
        {
            _inkscapeProvider = inkscapeProvider;
            _imageMagicProvider = imageMagicProvider;
            _processProvider = processProvider;
            _logger = logger;
        }
        
        public int ConvertSvgToIco(string svgFileName, int[] sizes, bool refresh)
        {
            var exitCode = 0;
            VerifySvgFile(svgFileName);

            var iconInfo = new IconInfo(svgFileName, sizes);

            if (refresh || iconInfo.NeedUpdate())
            {
                _inkscapeProvider.ExportSvgToPng(iconInfo.SvgFile, iconInfo.LargestPngFile);

                for (int i = 0; i < iconInfo.PngFiles.Count - 1; i++)
                {
                    var pngFile = iconInfo.PngFiles[i];
                    if (iconInfo.NeedUpdate(pngFile))
                    {
                        _imageMagicProvider.ResizePng(iconInfo.LargestPngFile, pngFile);
                    }
                    else
                    {
                        _logger.Info("Png up to date: " + pngFile.FullName);
                    }
                }
                _imageMagicProvider.CreateIconFromPngFiles(iconInfo);
            }
            else
            {
                _logger.Info("Icon up to date: " + iconInfo.IconFile.FullName);
            }
            return exitCode;
        }

        private void VerifySvgFile(string svgFileName)
        {
            if (!File.Exists(svgFileName))
                throw new FileNotFoundException("Svg file not found.", svgFileName);
        }
    }
}
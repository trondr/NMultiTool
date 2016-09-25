using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Common.Logging;
using NMultiTool.Library.Module.Commands.ConvertSvgToIco;

namespace NMultiTool.Library.Module.Commands.ConvertAllSvgToIco
{
    public class ConvertAllSvgToIcoCommandProvider : IConvertAllSvgToIcoCommandProvider
    {
        private readonly IIconInfoProvider _iconInfoProvider;
        private readonly IImageMagicProvider _imageMagicProvider;
        private readonly IInkscapeProvider _inkscapeProvider;
        private readonly IProcessProvider _processProvider;
        private readonly ILog _logger;

        public ConvertAllSvgToIcoCommandProvider(IIconInfoProvider iconInfoProvider,
            IImageMagicProvider imageMagicProvider,
            IInkscapeProvider inkscapeProvider,
            IProcessProvider processProvider,
            ILog logger
        )
        {
            _iconInfoProvider = iconInfoProvider;
            _imageMagicProvider = imageMagicProvider;
            _inkscapeProvider = inkscapeProvider;
            _processProvider = processProvider;
            _logger = logger;
        }

        public int ConvertAllSvgToIco(string folder, bool recursive, bool refresh, int[] sizes,
            int maxDegreeOfParallelism)
        {
            var resultCode = 0;
            var iconInfos = _iconInfoProvider.GetIconInfos(folder, recursive, sizes);
            var parallelOptions = new ParallelOptions() {MaxDegreeOfParallelism = maxDegreeOfParallelism};
            Parallel.ForEach(iconInfos, parallelOptions, (iconInfo, state) =>
            {
                try
                {
                    var isupdated = ConvertSvgToPngs(iconInfo, sizes, refresh);
                    var iconFileExists = iconInfo.IconFile.Exists;
                    if (isupdated || !iconFileExists || refresh)
                    {
                        _imageMagicProvider.CreateIconFromPngFiles(iconInfo);
                    }
                    else
                    {
                        _logger.Info("Up to date: " + iconInfo.IconFile.FullName);
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message);
                }
            });
            return resultCode;
        }

        private bool ConvertSvgToPngs(IconInfo iconInfo, int[] sizes, bool refresh)
        {
            var isUpdated = false;

            //Export from svg to png            
            bool needUpdate = iconInfo.NeedUpdate();
            if (needUpdate || refresh)
            {
                _inkscapeProvider.ExportSvgToPng(iconInfo.SvgFile, iconInfo.LargestPngFile);
                isUpdated = true;
            }
            
            //Resize the png to smaller sizes 
            for (var i = 0; i < iconInfo.PngFiles.Count - 1; i++)
            {
                if (needUpdate || refresh)
                {
                    var pngFile = iconInfo.PngFiles[i];
                    _imageMagicProvider.ResizePng(iconInfo.LargestPngFile, pngFile);
                    isUpdated = true;
                }
            }
            return isUpdated;
        }        
    }
}
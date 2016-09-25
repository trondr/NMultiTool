using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;
using Common.Logging;
using NMultiTool.Library.Module.Commands.ConvertAllSvgToIco;

namespace NMultiTool.Library.Module.Commands.ConvertSvgToIco
{
    public class ImageMagicProvider : IImageMagicProvider
    {
        private readonly IProcessProvider _processProvider;
        private readonly ILog _logger;

        public ImageMagicProvider(IProcessProvider processProvider, ILog logger)
        {
            _processProvider = processProvider;
            _logger = logger;
        }

        private string GetConvertExe()
        {
            var exe = ConfigurationManager.AppSettings["ConvertExe"];
            if (string.IsNullOrEmpty(exe))
            {
                throw new NMultiToolException("'ConvertExe' not specified in AppSettings in App.config");
            }
            if(!File.Exists(exe))
                throw new FileNotFoundException("Image Magic convert.exe was not found.", exe);
            return exe;
        }

        public void ResizePng(PngFileInfo sourcePngFile, PngFileInfo targetPngFile)
        {
            _logger.InfoFormat("Resizing: {0} -> {1}", sourcePngFile.Name, targetPngFile.Name);
            var convertExe = GetConvertExe();
            var arguments = string.Format("\"{0}\" -resize {1}x{1} \"{2}\"", sourcePngFile.FullName, targetPngFile.Size, targetPngFile.FullName);
            var exitCode = _processProvider.StartProcess(convertExe, arguments);
            if (exitCode != 0)
            {
                throw new NMultiToolException("Failed to resize png file. Exit code: " + exitCode);
            }
            if (!targetPngFile.Exists)
            {
                throw new NMultiToolException("Failed to resize png file. Png file has not been created: " + targetPngFile.FullName);
            }
            targetPngFile.ModfiedTime = sourcePngFile.ModfiedTime;
        }

        public void CreateIconFromPngFiles(IconInfo iconInfo)
        {
            _logger.InfoFormat("Creating: {0}", iconInfo.IconFile.FullName);
            var convertExe = GetConvertExe();
            var argumentsStringBuilder = new StringBuilder();
            foreach (var pngFile in iconInfo.PngFiles)
            {                
                argumentsStringBuilder.Append(string.Format("\"{0}\" ", pngFile.FullName));
            }
            argumentsStringBuilder.Append(string.Format("\"{0}\"", iconInfo.IconFile.FullName));
            var arguments = argumentsStringBuilder.ToString();
            var exitCode = _processProvider.StartProcess(convertExe, arguments);
            if (exitCode != 0)
            {
                throw new NMultiToolException("Failed to create icon from png files. Exit code: " + exitCode);
            }
            if (!iconInfo.IconFile.Exists)
            {
                throw new NMultiToolException("Failed to create icon from png files. Icon file has not been created: " + iconInfo.IconFile.FullName);
            }
            iconInfo.IconFile.ModfiedTime = iconInfo.SvgFile.ModfiedTime;
        }
    }
}
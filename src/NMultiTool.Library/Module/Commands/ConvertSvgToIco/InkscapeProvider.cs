using System.Configuration;
using System.IO;
using Common.Logging;
using NMultiTool.Library.Module.Commands.ConvertAllSvgToIco;
using NMultiTool.Library.Module.Common.Process;

namespace NMultiTool.Library.Module.Commands.ConvertSvgToIco
{
    public class InkscapeProvider : IInkscapeProvider
    {
        private readonly IProcessProvider _processProvider;
        private readonly ILog _logger;

        public InkscapeProvider(IProcessProvider processProvider, ILog logger)
        {
            _processProvider = processProvider;
            _logger = logger;
        }

        public void ExportSvgToPng(IconFileInfo svgFile, PngFileInfo pngFile)
        {
            _logger.InfoFormat("Exporting: {0}->{1}", svgFile.Name, pngFile.Name);
            var incscapeExe = GetInkscapeExe();
            var arguments = string.Format("-o \"{0}\" -w {2} -h {2} \"{1}\" ", pngFile.FullName, svgFile.FullName, pngFile.Size);
            var exitCode = ProcessOperation.StartConsoleProcess(incscapeExe, arguments, null, -1, null, _logger);
            if (exitCode != 0)
            {
                throw new NMultiToolException("Failed to export svg to png file. Exit code: " + exitCode);
            }
            if (!pngFile.Exists)
            {
                throw new NMultiToolException("Failed to export svg to png. Png file has not been created: " + pngFile.FullName);
            }
            pngFile.ModfiedTime = svgFile.ModfiedTime;
        }

        public void ExportSvgToPngs(IconInfo iconInfo)
        {
            foreach (var pngFileInfo in iconInfo.PngFiles)
            {
                if (iconInfo.NeedUpdate(pngFileInfo))
                {
                    ExportSvgToPng(iconInfo.SvgFile, pngFileInfo);
                }
                else
                {
                    _logger.Info("Png up to date: " + pngFileInfo.FullName);
                }
            }
        }

        private string GetInkscapeExe()
        {
            var exe = ConfigurationManager.AppSettings["InkscapeExe"];
            if (string.IsNullOrEmpty(exe))
            {
                throw new NMultiToolException("'InkscapeExe' not specified in AppSettings in App.config");
            }
            if (!File.Exists(exe))
            {
                throw new FileNotFoundException("Inkscape.exe was not found.", exe);
            }
            return exe;
        }
    }
}
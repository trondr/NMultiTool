using System.Diagnostics;
using System.IO;
using System.Text;
using Common.Logging;

namespace NMultiTool.Library.Module.Commands.ConvertSvgToIco
{
    public class ConvertSvgToIcoCommandProvider : IConvertSvgToIcoCommandProvider
    {
        private readonly IInkscapeProvider _inkscapeProvider;
        private readonly IImageMagicProvider _imageMagicProvider;

        private readonly ILog _logger;

        public ConvertSvgToIcoCommandProvider(IInkscapeProvider inkscapeProvider, IImageMagicProvider imageMagicProvider, ILog logger)
        {
            _inkscapeProvider = inkscapeProvider;
            _imageMagicProvider = imageMagicProvider;
            _logger = logger;
        }
        
        public int ConvertSvgToIco(string svgFileName)
        {
            VerifySvgFile(svgFileName);

            var baseFileName = GetBaseFileName(svgFileName);
            var iconFileName = GetIconFilename(baseFileName);
            var png256FileName = GetPngFileName(baseFileName, 256);
            var png128FileName = GetPngFileName(baseFileName, 128);
            var png64FileName = GetPngFileName(baseFileName, 64);
            var png32FileName = GetPngFileName(baseFileName, 32);
            var png16FileName = GetPngFileName(baseFileName, 16);

            ExportSvgToPng256(svgFileName, png256FileName);

            ResizePng(png256FileName, 128, png128FileName);
            ResizePng(png256FileName, 64, png64FileName);
            ResizePng(png256FileName, 32, png32FileName);
            ResizePng(png256FileName, 16, png16FileName);

            var pngFiles = new string[] { png16FileName, png32FileName, png64FileName, png128FileName, png256FileName };
            var exitCode = CreateIcon(pngFiles,iconFileName);

            return exitCode;
        }

        private int ResizePng(string png256FileName, int size, string pngFileName)
        {
            var convertExe = _imageMagicProvider.GetConvertExe();
            string arguments = string.Format("\"{0}\" -resize {1}x{1} \"{2}\"", png256FileName, size, pngFileName);
            var exitCode = StartProcess(convertExe, arguments);
            if (!File.Exists(pngFileName))
            {
                throw new NMultiToolException("Failed to resize png file. Png file has not been created: " + pngFileName);
            }
            return exitCode;
        }

        private int ExportSvgToPng256(string svgFileName, string png256FileName)
        {
            var incscapeExe = _inkscapeProvider.GetInkscapeExe();
            var arguments = string.Format("-z -e \"{0}\" -w 256 -h 256 \"{1}\" ", png256FileName, svgFileName);
            var exitCode = StartProcess(incscapeExe, arguments);
            if (!File.Exists(png256FileName))
            {
                throw new NMultiToolException("Failed to export svg to png. Png file has not been created: " + png256FileName);
            }
            return exitCode;
        }

        private int StartProcess(string exe, string arguments)
        {
            _logger.InfoFormat("\"{0}\" {1}", exe,arguments);
            var process = Process.Start(exe, arguments);
            if (process == null) throw new NMultiToolException("Failed to start convert.exe");
            process.WaitForExit();
            var exitCode = process.ExitCode;
            if (exitCode == 0)
            {
                _logger.Info("ExitCode: " + exitCode);
            }
            else
            {
                _logger.Warn("ExitCode: " + exitCode);
            }
            return process.ExitCode;
        }
        
        private int CreateIcon(string[] pngFiles, string iconFileName)
        {
            var convertExe = _imageMagicProvider.GetConvertExe();
            var argumentsStringBuilder = new StringBuilder();
            foreach (var pngFile in pngFiles)
            {
                argumentsStringBuilder.Append(string.Format("\"{0}\" ", pngFile));
            }
            argumentsStringBuilder.Append(string.Format("\"{0}\"", iconFileName));
            var arguments = argumentsStringBuilder.ToString();
            var process = Process.Start(convertExe, arguments);
            if (process == null) throw new NMultiToolException("Failed to start convert.exe");
            process.WaitForExit();
            return process.ExitCode;
        }

        private string GetPngFileName(string baseFileName, int size)
        {
            var pngFileName = baseFileName + "-" + size + ".png";
            return pngFileName;
        }

        private string GetIconFilename(string baseFileName)
        {
            var iconFileName = baseFileName + ".ico";
            return iconFileName;
        }

        private string GetBaseFileName(string fileName)
        {
            var fullPath = Path.GetFullPath(fileName);
            var file = new FileInfo(fullPath);
            if (file.Directory != null)
                return Path.Combine(file.Directory.FullName, Path.GetFileNameWithoutExtension(fileName));
            return Path.GetFileNameWithoutExtension(fullPath);
        }

        private void VerifySvgFile(string svgFileName)
        {
            if (!File.Exists(svgFileName))
                throw new FileNotFoundException("Svg file not found.", svgFileName);
        }
    }
}
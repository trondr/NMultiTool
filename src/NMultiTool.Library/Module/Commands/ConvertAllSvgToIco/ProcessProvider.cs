using System.Diagnostics;
using Common.Logging;
using NMultiTool.Library.Module.Commands.ConvertSvgToIco;

namespace NMultiTool.Library.Module.Commands.ConvertAllSvgToIco
{
    public class ProcessProvider : IProcessProvider
    {
        private readonly ILog _logger;

        public ProcessProvider(ILog logger)
        {
            _logger = logger;
        }

        public int StartProcess(string exe, string arguments)
        {
            _logger.DebugFormat("\"{0}\" {1}", exe, arguments);
            var startInfo = new ProcessStartInfo()
            {
                FileName = exe,
                Arguments = arguments,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden
            };
            var process = Process.Start(startInfo);
            if (process == null) throw new NMultiToolException("Failed to start: " + exe);
            process.WaitForExit();
            var exitCode = process.ExitCode;
            if (exitCode == 0)
            {
                _logger.DebugFormat("ExitCode: " + exitCode);
            }
            else
            {
                _logger.Warn("ExitCode: " + exitCode);
            }
            return process.ExitCode;
        }
    }
}
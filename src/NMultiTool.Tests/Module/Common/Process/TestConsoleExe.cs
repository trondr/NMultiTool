using System;
using System.IO;
using System.Threading;
using NMultiTool.Library.Module.Common.Resources;

namespace NMultiTool.Tests.Module.Common.Process
{
    internal class TestConsoleExe : IDisposable
    {
        private string _testConsoleExeFile;
        private bool _disposed = false;

        public string TestConsoleExeFile
        {
            get
            {
                if (string.IsNullOrEmpty(_testConsoleExeFile))
                {
                    var tempExe = Path.GetTempFileName() + ".exe";
                    _testConsoleExeFile = Path.Combine(Path.GetTempPath(), tempExe);
                    var embededResource = new EmbeddedResource();
                    embededResource.ExtractToFile("NMultiTool.Tests.Module.Common.Process.NMultiTool.TestConsole.exe", typeof(ProcessTests).Assembly, _testConsoleExeFile);
                    if (!File.Exists(_testConsoleExeFile))
                    {
                        throw new FileNotFoundException("Test console not found: " + _testConsoleExeFile);
                    }
                }
                return _testConsoleExeFile;
            }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                if (File.Exists(_testConsoleExeFile))
                {
                    Thread.Sleep(1000);
                    File.Delete(_testConsoleExeFile);
                }
                _disposed = true;
            }
        }
    }
}
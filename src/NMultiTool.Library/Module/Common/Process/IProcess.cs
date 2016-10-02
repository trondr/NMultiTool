using System;
using System.Windows.Input;

namespace NMultiTool.Library.Module.Common.Process
{
    public interface IProcess
    {
        void Execute(string fileName, string arguments, bool waitforExit, string workingDirectory);        
        string StandardOutput { get; }
        string StandardError { get; }
        int ExitCode { get; }
    }
}

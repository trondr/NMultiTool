﻿using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using NMultiTool.Library.Module.Commands.ConvertSvgToIco;

namespace NMultiTool.Library.Module.Common.Process
{
    public class Process : IProcess
    {
        readonly StringBuilder _standardOutputText;
        readonly StringBuilder _standardErrorText;
        readonly List<string> _standardInputLines;
        bool _canExecute;

        public Process()
        {
            _standardOutputText = new StringBuilder();
            _standardErrorText = new StringBuilder();
            _standardInputLines = new List<string>();
            _canExecute = true;
        }
        
        public void Execute(string fileName, string arguments, bool waitforExit, string workingDirectory)
        {
            CheckAndSetCanExecute();
            var processStartInfo = new ProcessStartInfo(fileName,arguments);
            processStartInfo.UseShellExecute = false;
            processStartInfo.RedirectStandardOutput = true;
            processStartInfo.RedirectStandardError = true;
            processStartInfo.RedirectStandardInput = true;
            processStartInfo.CreateNoWindow = true;
            var process = new System.Diagnostics.Process
            {
                StartInfo = processStartInfo,
                EnableRaisingEvents = true
            };
            process.OutputDataReceived += (sendingProcess, standardOutputLine) =>
            {
                if(standardOutputLine.Data != null)
                    _standardOutputText.AppendLine(standardOutputLine.Data);
            };
            process.ErrorDataReceived += (sendingProcess, errorLine) =>
            {
                if(errorLine.Data != null)
                    _standardErrorText.AppendLine(errorLine.Data);
            };
            process.Exited += (sender, args) =>
            {
                ExitCode = process.ExitCode;                
            };            
            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            foreach (var standardInputLine in _standardInputLines)
            {
                process.StandardInput.WriteLine(standardInputLine);
            }
            if (waitforExit)
            {
                process.WaitForExit();                
            }
        }

        private void CheckAndSetCanExecute()
        {
            if(!_canExecute)
                throw new NMultiToolException("Cannot execute process before previous process has been reset.");
            _canExecute = false;
        }

        public void Reset()
        {
            _standardErrorText.Clear();
            _standardOutputText.Clear();
            _standardInputLines.Clear();
            _canExecute = true;
        }

        public string StandardOutput => _standardOutputText.ToString();
        public string StandardError => _standardErrorText.ToString();
        public int ExitCode { get; private set; }

        public void WriteToStandardInput(string standardInputLine)
        {
            _standardInputLines.Add(standardInputLine);
        }
    }
}
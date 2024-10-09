// File: Process.cs
// Project Name: NNMultiTool
// Project Home: https://github.com/trondr/NMultiTool
// License: New BSD License (BSD) https://github.com/trondr/NMultiTool/blob/master/LICENSE
// Credits: See the Credit folder in this project
// Copyright © <github.com/trondr> 2024
// All rights reserved.

using System;
using System.Diagnostics;
using System.Text;
using Common.Logging;

namespace NMultiTool.Library.Module.Common.Process
{
    
    public class ProcessOperation
    {
        
        public static int StartConsoleProcess(string fileName, string arguments, string workingDirectory, int timeout, string inputData, ILog logger)
        {
            logger.Info($"START: '{fileName}' {arguments}");

            var startInfo = new ProcessStartInfo
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                FileName = fileName
            };

            if (!string.IsNullOrWhiteSpace(arguments))
            {
                startInfo.Arguments = arguments;
            }

            if (!string.IsNullOrWhiteSpace(workingDirectory))
            {
                startInfo.WorkingDirectory = workingDirectory;
            }

            using (var consoleProcess = new System.Diagnostics.Process())
            {
                consoleProcess.StartInfo = startInfo;

                if (!string.IsNullOrWhiteSpace(inputData))
                {
                    consoleProcess.StandardInput.Write(inputData);
                    consoleProcess.StandardInput.Close();
                }

                var stdOutBuilder = new StringBuilder();
                var stdErrorBuilder = new StringBuilder();

                consoleProcess.OutputDataReceived += (sender, e) => stdOutBuilder.AppendLine(e.Data);
                consoleProcess.ErrorDataReceived += (sender, e) => stdErrorBuilder.AppendLine(e.Data);

                consoleProcess.EnableRaisingEvents = true;
                consoleProcess.Start();
                consoleProcess.BeginOutputReadLine();
                consoleProcess.BeginErrorReadLine();

                if (!consoleProcess.WaitForExit(timeout))
                {
                    consoleProcess.Close();
                    throw new Exception($"Process execution timed out: \"{fileName}\" {arguments}");
                }

                consoleProcess.CancelOutputRead();
                consoleProcess.CancelErrorRead();

                var processExitData = new
                {
                    FileName = fileName,
                    Arguments = arguments,
                    ExitCode = consoleProcess.ExitCode,
                    StdOutput = stdOutBuilder.ToString().Trim(),
                    StdError = stdErrorBuilder.ToString().Trim()
                };
                logger.Info($"StdOut:{Environment.NewLine}:{processExitData.StdOutput}");
                logger.Info($"StdError:{Environment.NewLine}:{processExitData.StdError}");
                logger.Info($"Exit: {processExitData.ExitCode}");
                return processExitData.ExitCode;
            }
        }

    }

}

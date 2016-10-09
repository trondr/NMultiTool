// File: Process.cs
// Project Name: NNMultiTool
// Project Home: https://github.com/trondr/NMultiTool
// License: New BSD License (BSD) https://github.com/trondr/NMultiTool/blob/master/LICENSE
// Credits: See the Credit folder in this project
// Copyright © <github.com/trondr> 2016
// All rights reserved.


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
        void Reset();
    }
}

using System;
using System.Linq;
using Common.Logging;
using NCmdLiner.Attributes;
using NMultiTool.Library.Infrastructure;
using NMultiTool.Library.Module.Commands.InstallUtil;

namespace NMultiTool.Module.Commands
{
    public class InstallUtilCommand: CommandDefinition
    {
        private readonly IInstallUtil _installUtil;
        private readonly ILog _logger;

        public InstallUtilCommand(IInstallUtil installUtil, ILog logger)
        {
            _installUtil = installUtil;
            _logger = logger;
        }

        [CLSCompliant(false)]
        [Command(Description = "InstallUtil processes specified .NET assemblies and executes Install or Uninstall on the installer classes defined in the assemblies.")]
        public int InstallUtil(
            [RequiredCommandParameter(Description = "Install action. Valid values are Install|UnInstall", ExampleValue = InstallAction.Install, AlternativeName = "a")]
            InstallAction installAction,
            [RequiredCommandParameter(Description = "Directory with .NET components to process.", ExampleValue = @"c:\Program Fils (x86)\SomeCompany\SomeApplication\1.0.0.0", AlternativeName = "d")]
            string directory,
            [RequiredCommandParameter(Description = "Array of file specifications to include, i.e all files matching the include file specfication will be processed.", AlternativeName = "if", ExampleValue = new string[] { "SomeCompany.SomeApplication*.dll", "SomeCompany.SomeApplication*.exe" })]
            string[] includeFileSpecs,
            [RequiredCommandParameter(Description = "Array of file specifications to exclude, i.e all files matching the exclude file specfication will not be processed.",  AlternativeName = "ef", ExampleValue = new string[] { "Microsoft.*.dll", "Microsoft.*.exe"})]
            string[] excludeFileSpecs
            )
        {
            _logger.Info("Start InstallUtil...");            
            var returnValue = _installUtil.Execute(installAction, directory, includeFileSpecs.ToList(), excludeFileSpecs.ToList());            
            _logger.Info("Stop InstallUtil.");
            return returnValue;
        }
    }
}

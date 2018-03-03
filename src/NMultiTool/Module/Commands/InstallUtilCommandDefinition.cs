using System;
using System.Linq;
using Common.Logging;
using NCmdLiner;
using NCmdLiner.Attributes;
using NMultiTool.Library.Infrastructure;
using NMultiTool.Library.Module.Commands.InstallUtil;

namespace NMultiTool.Module.Commands
{
    public class InstallUtilCommandDefinition: CommandDefinition
    {
        private readonly IInstallUtilCommandProviderFactory _installUtilCommandProviderFactory;
        private readonly ILog _logger;

        public InstallUtilCommandDefinition(IInstallUtilCommandProviderFactory installUtilCommandProviderFactory, ILog logger)
        {
            _installUtilCommandProviderFactory = installUtilCommandProviderFactory;
            _logger = logger;
        }

        [CLSCompliant(false)]
        [Command(Description = "InstallUtil processes specified .NET assemblies and executes Install or Uninstall on the installer classes defined in the assemblies.")]
        public Result<int> InstallUtil(
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
            var installUtilCommandProvider = _installUtilCommandProviderFactory.GetInstallUtilCommandProvider();
            int returnValue = 0;
            try
            {
                _logger.Info("Start InstallUtil...");
                installUtilCommandProvider.InstallUtil(installAction, directory, includeFileSpecs.ToList(), excludeFileSpecs.ToList());                
            }
            catch (Exception e)
            {
                return Result.Fail<int>(e);
            }
            finally
            {
                _installUtilCommandProviderFactory.Release(installUtilCommandProvider);
                _logger.Info("Stop InstallUtil.");
            }
            return Result.Ok(returnValue);
        }
    }
}

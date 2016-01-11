using System.Reflection;
using Common.Logging;
using NCmdLiner.Attributes;
using NMultiTool.Library.Commands.CreateNugetSpec;
using NMultiTool.Library.Common;

namespace NMultiTool.Commands
{
    public class CreateNugetSpecCommand: CommandsBase
    {
        private readonly INuGetSpec _nuGetSpec;
        private readonly ILog _logger;

        public CreateNugetSpecCommand(INuGetSpec nuGetSpec,ILog logger)
        {
            _nuGetSpec = nuGetSpec;
            _logger = logger;
        }

        [Command(Description = "Create nuget spec from a template and update information in the resulting nuget file with meta data from a specified assembly.")]
        public void CreateNugetSpec(
            [RequiredCommandParameter(
                Description = "The nuget manifest template file.",
                ExampleValue = "c:\\temp\\SomeSource.nuspec",
                AlternativeName = "nmt"
                )] string manifestTemplate,
            [RequiredCommandParameter(
                Description = "The nuget manifest target file.",
                ExampleValue = "c:\\temp\\SomeTarget.nuspec",
                AlternativeName = "tf"
                )] string manifestTarget,
            [RequiredCommandParameter(
                Description = "The assembly with meta data to write into the nuget manifest.",
                ExampleValue = "c:\\temp\\SomeLib.dll",
                AlternativeName = "ap"
                )] string assemblyPath
            )
        {
            _logger.Info("Start CreateNugetSpec...");
            _nuGetSpec.Create(manifestTemplate, manifestTarget, assemblyPath);
            _logger.Info("Stop CreateNugetSpec.");
        }
    }
}

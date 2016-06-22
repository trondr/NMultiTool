using NCmdLiner.Attributes;
using NMultiTool.Library.Infrastructure;
using NMultiTool.Library.Module.Commands.ExtractIcon;

namespace NMultiTool.Module.Commands
{
    public class ExtractIconCommand : CommandDefinition
    {
        private readonly IExtractIconCommandProvider _extractIconCommandProvider;

        public ExtractIconCommand(IExtractIconCommandProvider extractIconCommandProvider)
        {
            _extractIconCommandProvider = extractIconCommandProvider;
        }

        [Command(Description = "Extract icons from file.")]
        public int ExtractIcon(
            [RequiredCommandParameter(Description = "Binary file to extract icons from.",AlternativeName = "sf", ExampleValue = @"c:\windows\notepad.exe")]
            string sourceFile, 
            [RequiredCommandParameter(Description = "Save icon to this folder.",AlternativeName = "tf", ExampleValue = @"c:\temp")]
            string targetFolder
            )
        {

            return _extractIconCommandProvider.ExtratIcon(sourceFile, targetFolder);
        }
    }
}

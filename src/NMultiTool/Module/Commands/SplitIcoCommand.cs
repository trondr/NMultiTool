using NCmdLiner.Attributes;
using NMultiTool.Library.Infrastructure;
using NMultiTool.Library.Module.Commands.SplitIco;

namespace NMultiTool.Module.Commands
{
    public class SplitIcoCommand : CommandDefinition
    {
        private readonly ISplitIcoCommandProvider _splitIcoCommandProvider;

        public SplitIcoCommand(ISplitIcoCommandProvider splitIcoCommandProvider)
        {
            _splitIcoCommandProvider = splitIcoCommandProvider;
        }

        [Command(Description = "Split an icon file into multiple images")]
        public int SplitIco(
            [RequiredCommandParameter(
                 Description = "Split ico file in multiple png files", 
                 AlternativeName = "ico", 
                 ExampleValue = "c:\\temp\\test.ico")]
            string icoFileName
        )
        {
            return _splitIcoCommandProvider.SplitIco(icoFileName);
        }
    }
}
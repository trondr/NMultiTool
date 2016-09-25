using NCmdLiner.Attributes;
using NMultiTool.Library.Infrastructure;
using NMultiTool.Library.Module.Commands.ConvertAllSvgToIco;

namespace NMultiTool.Module.Commands
{
    public class ConvertAllSvgToIcoCommand : CommandDefinition
    {
        private readonly IConvertAllSvgToIcoCommandProvider _convertAllSvgToIcoCommandProvider;

        public ConvertAllSvgToIcoCommand(IConvertAllSvgToIcoCommandProvider convertAllSvgToIcoCommandProvider)
        {
            _convertAllSvgToIcoCommandProvider = convertAllSvgToIcoCommandProvider;
        }

        [Command(Description = "Convert svg files in folder (and subfolders if recursive==true) to multi size icon files of specified sizes.")]
        public int ConvertAllSvgToIco(
            [RequiredCommandParameter(Description = "Folder to search for svg files",AlternativeName = "d", ExampleValue = @"c:\temp\icons")]
            string folder,
            [OptionalCommandParameter(Description = "Recurse sub folders.", AlternativeName = "r", DefaultValue = true, ExampleValue = true)]
            bool recursive,
            [OptionalCommandParameter(Description = "Recreate icon even if it allready exists.",AlternativeName = "rf", DefaultValue = false, ExampleValue = false)]
            bool refresh,
            [OptionalCommandParameter(Description = "Create multi size icon with the specified array of sizes", AlternativeName = "s", ExampleValue = new int[] {16, 32, 64, 128, 256}, DefaultValue = new int[] {16, 32, 64, 128, 256})]
            int[] sizes,
            [OptionalCommandParameter(Description = "Run specified number of conversions in parallel.", AlternativeName = "p", DefaultValue = 4, ExampleValue = 4)]
            int maxDegreeOfParallelism)
        {
            var exitCode = _convertAllSvgToIcoCommandProvider.ConvertAllSvgToIco(folder, recursive, refresh,
                sizes, maxDegreeOfParallelism);
            return exitCode;
        }
    }
}

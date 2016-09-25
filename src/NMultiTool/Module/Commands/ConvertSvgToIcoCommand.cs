using NCmdLiner.Attributes;
using NMultiTool.Library.Infrastructure;
using NMultiTool.Library.Module.Commands.ConvertSvgToIco;

namespace NMultiTool.Module.Commands
{
    public class ConvertSvgToIcoCommand : CommandDefinition
    {
        private readonly IConvertSvgToIcoCommandProvider _convertSvgToIcoCommandProvider;

        public ConvertSvgToIcoCommand(IConvertSvgToIcoCommandProvider convertSvgToIcoCommandProvider)
        {
            _convertSvgToIcoCommandProvider = convertSvgToIcoCommandProvider;
        }

        [Command(Description = "Convert a .svg vector graphics file to a multi size .ico file.")]
        public int ConvertSvgToIco(
            [RequiredCommandParameter(
                Description = "Path to vector graphics file having format *.svg", 
                AlternativeName = "svg", 
                ExampleValue = "c:\\temp\\test.svg")]
            string svgFileName,
            
            [OptionalCommandParameter(
                Description = "Recreate icon even if it allready exists.", 
                AlternativeName = "rf", 
                DefaultValue = false, 
                ExampleValue = false)]
            bool refresh,
            
            [OptionalCommandParameter(
                Description = "Sizes of the images included in the multi size .ico file.", 
                AlternativeName = "s", 
                ExampleValue = new[] {16, 32, 64, 128, 256}, 
                DefaultValue = new[] {16, 32, 64, 128, 256})]
            int[] sizes
            )
        {
            return _convertSvgToIcoCommandProvider.ConvertSvgToIco(svgFileName, sizes, refresh);
        }
    }
}
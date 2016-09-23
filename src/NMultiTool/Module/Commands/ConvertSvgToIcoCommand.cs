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

        [Command(Description = "Convert a .svg vector graphics file to .ico icon file with 16x16, 32x32, 64x64, 128x128, 256x256.")]
        public int ConvertSvgToIco(
            [RequiredCommandParameter(Description = "Path to vector graphics file having format *.svg", AlternativeName = "svg", ExampleValue = "c:\\temp\\test.svg")]
            string svgFileName
            )
        {
            return _convertSvgToIcoCommandProvider.ConvertSvgToIco(svgFileName);
        }
    }
}
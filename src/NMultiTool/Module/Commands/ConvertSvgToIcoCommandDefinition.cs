using NCmdLiner;
using NCmdLiner.Attributes;
using NMultiTool.Library.Infrastructure;
using NMultiTool.Library.Module.Commands.ConvertSvgToIco;

namespace NMultiTool.Module.Commands
{
    public class ConvertSvgToIcoCommandDefinition : CommandDefinition
    {
        private readonly IConvertSvgToIcoCommandProviderFactory _convertSvgToIcoCommandProviderFactory;
        
        public ConvertSvgToIcoCommandDefinition(IConvertSvgToIcoCommandProviderFactory convertSvgToIcoCommandProviderFactory)
        {
            _convertSvgToIcoCommandProviderFactory = convertSvgToIcoCommandProviderFactory;            
        }

        [Command(Description = "Convert a .svg vector graphics file to a multi size .ico file.")]
        public Result<int> ConvertSvgToIco(
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
            var convertSvgToIcoCommandProvider = _convertSvgToIcoCommandProviderFactory.GetConvertSvgToIcoCommandProvider();
            try
            {
                return convertSvgToIcoCommandProvider.ConvertSvgToIco(svgFileName, sizes, refresh);
            }
            finally
            {
                _convertSvgToIcoCommandProviderFactory.Release(convertSvgToIcoCommandProvider);
            }
        }
    }
}
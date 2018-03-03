using NCmdLiner;
using NCmdLiner.Attributes;
using NMultiTool.Library.Infrastructure;
using NMultiTool.Library.Module.Commands.SplitIco;

namespace NMultiTool.Module.Commands
{
    public class SplitIcoCommandDefinition : CommandDefinition
    {
        private readonly ISplitIcoCommandProviderFactory _splitIcoCommandProviderFactory;
        

        public SplitIcoCommandDefinition(ISplitIcoCommandProviderFactory splitIcoCommandProviderFactory)
        {
            _splitIcoCommandProviderFactory = splitIcoCommandProviderFactory;
            
        }

        [Command(Description = "Split an icon file into multiple images")]
        public Result<int> SplitIco(
            [RequiredCommandParameter(
                 Description = "Split ico file in multiple png files", 
                 AlternativeName = "ico", 
                 ExampleValue = "c:\\temp\\test.ico")]
            string icoFileName
        )
        {
            var splitCommandProvider = _splitIcoCommandProviderFactory.GetSplitIcoCommandProvider();
            try
            {
                return splitCommandProvider.SplitIco(icoFileName);
            }
            finally
            {
                _splitIcoCommandProviderFactory.Release(splitCommandProvider);
            }
        }
    }
}
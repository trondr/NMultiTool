using System.Configuration;
using System.IO;

namespace NMultiTool.Library.Module.Commands.ConvertSvgToIco
{
    public class InkscapeProvider : IInkscapeProvider
    {
        public string GetInkscapeExe()
        {
            var exe = ConfigurationManager.AppSettings["InkscapeExe"];
            if (string.IsNullOrEmpty(exe))
            {
                throw new NMultiToolException("'InkscapeExe' not specified in AppSettings in App.config");
            }
            if(!File.Exists(exe))
                throw new FileNotFoundException("Inkscape.exe was not found.", exe);
            return exe;
        }
    }
}
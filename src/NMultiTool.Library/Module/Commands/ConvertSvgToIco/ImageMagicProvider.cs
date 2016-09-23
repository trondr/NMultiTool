using System.Configuration;
using System.IO;

namespace NMultiTool.Library.Module.Commands.ConvertSvgToIco
{
    public class ImageMagicProvider : IImageMagicProvider
    {
        public string GetConvertExe()
        {
            var exe = ConfigurationManager.AppSettings["ConvertExe"];
            if (string.IsNullOrEmpty(exe))
            {
                throw new NMultiToolException("'ConvertExe' not specified in AppSettings in App.config");
            }
            if(!File.Exists(exe))
                throw new FileNotFoundException("Image Magic convert.exe was not found.", exe);
            return exe;
        }
    }
}
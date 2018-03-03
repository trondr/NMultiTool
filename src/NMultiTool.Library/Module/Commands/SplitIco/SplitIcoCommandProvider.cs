using System.IO;
using ImageMagick;
using NCmdLiner;

namespace NMultiTool.Library.Module.Commands.SplitIco
{
    public class SplitIcoCommandProvider : ISplitIcoCommandProvider
    {
        public Result<int> SplitIco(string icoFileName)
        {
            var fullName = Path.GetFullPath(icoFileName);
            var folder = Path.GetDirectoryName(fullName);
            var baseName = Path.GetFileNameWithoutExtension(fullName);
            using (var imageCollection = new MagickImageCollection())
            {
                imageCollection.Read(icoFileName);
                foreach (var image in imageCollection)
                {                    
                    var pngFileName = baseName + "-" + image.Height + ".png";
                    var pngFile = Path.Combine(folder, pngFileName);
                    image.Write(pngFile);
                }
            }
            return Result.Ok(0);
        }
    }
}
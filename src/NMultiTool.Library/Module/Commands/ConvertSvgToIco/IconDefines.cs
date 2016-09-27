using System.Collections.Generic;
using ImageMagick;

namespace NMultiTool.Library.Module.Commands.ConvertSvgToIco
{
    public class IconDefines : IWriteDefines
    {
        public IconDefines(int[] iconInfoSizes)
        {
            var sizesString = string.Join(",", iconInfoSizes); //Example: "256,128,64,32,16"
            var defineList = new List<IDefine> { new MagickDefine(MagickFormat.Ico, "auto-resize", sizesString) };
            Defines = defineList;
            Format = MagickFormat.Ico;
        }

        public IEnumerable<IDefine> Defines { get; }

        public MagickFormat Format { get; }
    }
}
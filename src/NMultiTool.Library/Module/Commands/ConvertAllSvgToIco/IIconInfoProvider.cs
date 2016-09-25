using System.Collections.Generic;

namespace NMultiTool.Library.Module.Commands.ConvertAllSvgToIco
{
    public interface IIconInfoProvider
    {
        IEnumerable<IconInfo> GetIconInfos(string folder, bool recursive, int[] sizes);
    }
}
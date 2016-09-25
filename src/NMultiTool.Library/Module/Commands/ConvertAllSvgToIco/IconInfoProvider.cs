using System;
using System.Collections.Generic;
using System.IO;

namespace NMultiTool.Library.Module.Commands.ConvertAllSvgToIco
{
    public class IconInfoProvider : IIconInfoProvider
    {
        public IEnumerable<IconInfo> GetIconInfos(string folder, bool recursive, int[] sizes)
        {
            if (folder == null) throw new ArgumentNullException(nameof(folder));
            if(!Directory.Exists(folder)) throw new DirectoryNotFoundException("Folder not found: " + folder);

            var subFolders = Directory.GetDirectories(folder);
            var svgFiles = Directory.GetFiles(folder, "*.svg", SearchOption.TopDirectoryOnly);
            foreach (var svgFile in svgFiles)
            {
                yield return new IconInfo(svgFile, sizes);
            }
            if (recursive)
            {
                foreach (var subFolder in subFolders)
                {
                    foreach (var svgFile in GetIconInfos(subFolder, true, sizes))
                    {
                        yield return svgFile;
                    }                    
                }
            }
        }
    }
}
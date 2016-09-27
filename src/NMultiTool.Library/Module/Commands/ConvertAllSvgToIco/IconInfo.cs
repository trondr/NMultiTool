using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;

namespace NMultiTool.Library.Module.Commands.ConvertAllSvgToIco
{
    public class IconInfo
    {
        public IconInfo(string svgFileName, int[] sizes)
        {
            SvgFile = new IconFileInfo(svgFileName);
            Sizes = sizes;            
        }

        public int[] Sizes { get; private set; }

        public List<PngFileInfo> PngFiles
        {
            get
            {
                if (_pngFiles == null)
                {
                    _pngFiles = new List<PngFileInfo>();
                    var sortedSizes = Sizes.OrderBy(i => i);
                    foreach (var size in sortedSizes)
                    {
                        var pngFileName = GetPngFileName(size);
                        var pngFile = new PngFileInfo(pngFileName, size);
                        _pngFiles.Add(pngFile);
                    }   
                }
                return _pngFiles;
            }
        }        
        private List<PngFileInfo> _pngFiles;

        public IconFileInfo SvgFile { get; }
        
        public IconFileInfo IconFile
        {
            get
            {
                if (_iconFile == null)
                {
                    _iconFile = new IconFileInfo(GetIconName(BaseFileName));
                }
                return _iconFile;
            }            
        }
        private IconFileInfo _iconFile;

        public PngFileInfo LargestPngFile
        {
            get
            {
                if (_largestPngFile == null)
                {
                    _largestPngFile = PngFiles.Last();
                }
                return _largestPngFile;
            }
        }
        private  PngFileInfo _largestPngFile;

        public bool NeedUpdate()
        {
            var modifyDateDifferent = IconFile.ModfiedTime < SvgFile.ModfiedTime;
            if (modifyDateDifferent)
                return true;

            var icoFileIsMissing = !IconFile.Exists;
            if (icoFileIsMissing)
                return true;

            var pngFilesAreMissing = PngFiles.Any(info => !info.Exists);
            if (pngFilesAreMissing)
                return true;

            modifyDateDifferent = PngFiles.Any(info => info.ModfiedTime < SvgFile.ModfiedTime);
            if (modifyDateDifferent)
                return true;

            return false;
        }

        private string BaseFileName
        {
            get
            {
                if (string.IsNullOrEmpty(_baseName))
                {
                    _baseName = GetBaseFileName();
                }
                return _baseName;
            }
        }
        private string _baseName;
        

        private string GetPngFileName(int size)
        {
            var pngFileName = BaseFileName + "-" + size + ".png";
            return pngFileName;
        }

        private string GetIconName(string baseName)
        {
            var iconFileName = baseName + ".ico";
            return iconFileName;
        }

        private string GetBaseFileName()
        {
            var file = new FileInfo(SvgFile.FullName);
            var baseName = file.Directory != null ? 
                Path.Combine(file.Directory.FullName, Path.GetFileNameWithoutExtension(SvgFile.FullName)) : 
                Path.GetFileNameWithoutExtension(SvgFile.FullName);
            return baseName;
        }        
    }
}
using System;
using System.IO;

namespace NMultiTool.Library.Module.Commands.ConvertAllSvgToIco
{
    public class IconFileInfo
    {
        public IconFileInfo(string fullName)
        {
            FullName = GetFullName(fullName);
        }

        public string FullName { get; }

        public string Name
        {
            get
            {
                if (string.IsNullOrEmpty(_name))
                {
                    _name = Path.GetFileName(FullName);
                }
                return _name;
            }
        }
        private string _name;

        public DateTime ModfiedTime
        {
            get
            {
                return File.GetLastWriteTime(FullName);                
            }
            set
            {
                File.SetLastWriteTime(FullName, value);
            }
        }

        public bool Exists => File.Exists(FullName);

        private string GetFullName(string fullName)
        {
            var expandedFullName = Environment.ExpandEnvironmentVariables(fullName);
            expandedFullName = Path.GetFullPath(expandedFullName);
            return expandedFullName;
        }
    }
}
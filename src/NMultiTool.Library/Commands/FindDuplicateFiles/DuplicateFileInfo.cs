using System.IO;

namespace NMultiTool.Library.Commands.FindDuplicateFiles
{
    public class DuplicateFileInfo
    {
        public DuplicateFileInfo(FileInfo fileInfo)
        {
            FullName = fileInfo.FullName;
            Length = fileInfo.Length;
        }
        public string FullName { get; set; }
        public long Length { get; set; }
        public string Sha1Mini { get; set; }
        public string Sha1 { get; set; }

        public override int GetHashCode()
        {
            return this.FullName.GetHashCode();
        }

        public override string ToString()
        {
            return FullName;
        }

        public override bool Equals(object obj)
        {
            return FullName.Equals(obj);
        }
    }
}
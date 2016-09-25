namespace NMultiTool.Library.Module.Commands.ConvertAllSvgToIco
{
    public class PngFileInfo : IconFileInfo
    {
        public PngFileInfo(string fullName, int size) : base(fullName)
        {
            Size = size;
        }
        public int Size { get; }
    }
}
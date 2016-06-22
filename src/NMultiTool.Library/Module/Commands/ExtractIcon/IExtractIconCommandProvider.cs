using System.IO;
using System.Windows.Media.Imaging;

namespace NMultiTool.Library.Module.Commands.ExtractIcon
{
    public interface IExtractIconCommandProvider
    {
        int ExtratIcon(string sourceFile, string targetFolder);
    }

    public class ExtractIconCommandProvider : IExtractIconCommandProvider
    {
        public int ExtratIcon(string sourceFile, string targetFolder)
        {
            BitmapSource bmpSrc;
            using (var sysicon = System.Drawing.Icon.ExtractAssociatedIcon(sourceFile))
            {
                bmpSrc = System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(
                            sysicon.Handle,
                            System.Windows.Int32Rect.Empty,
                            System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
            }
            var sourceFileInfo = new FileInfo(sourceFile);
            var baseFileName =  Path.GetFileNameWithoutExtension(sourceFileInfo.Name);
            var iconFile = Path.Combine(targetFolder, baseFileName + ".ico");
            using (var fileStream = new FileStream(iconFile, FileMode.Create))
            {
                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bmpSrc));
                encoder.Save(fileStream);
            }
            return 0;
        }
    }
}

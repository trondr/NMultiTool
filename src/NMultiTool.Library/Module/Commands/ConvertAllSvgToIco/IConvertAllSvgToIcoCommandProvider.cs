using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace NMultiTool.Library.Module.Commands.ConvertAllSvgToIco
{
    public interface IConvertAllSvgToIcoCommandProvider
    {
        int ConvertAllSvgToIco(string folder, bool recursive, bool refresh, int[] sizes, int maxDegreeOfParallelism);
    }
}

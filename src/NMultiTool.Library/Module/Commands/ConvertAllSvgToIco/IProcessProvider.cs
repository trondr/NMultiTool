namespace NMultiTool.Library.Module.Commands.ConvertAllSvgToIco
{
    public interface IProcessProvider
    {
        int StartProcess(string exe, string arguments);
    }
}
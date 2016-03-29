namespace NMultiTool.Library.Module.Commands.Folder2Wxs
{
    public interface IIdFromNameGenerator
    {
        string GetId(string name, string postfix);
    }
}
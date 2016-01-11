namespace NMultiTool.Library.Commands.Folder2Wxs
{
    public interface IIdFromNameGenerator
    {
        string GetId(string name, string postfix);
    }
}
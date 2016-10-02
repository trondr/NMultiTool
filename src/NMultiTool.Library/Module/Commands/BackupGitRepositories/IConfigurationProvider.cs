namespace NMultiTool.Library.Module.Commands.BackupGitRepositories
{
    public interface IConfigurationProvider<T>
    {
        T Load(string configurationFile);

        void Save(T configuration, string configurationFile);
    }
}
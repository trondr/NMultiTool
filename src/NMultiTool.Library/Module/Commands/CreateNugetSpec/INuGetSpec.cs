namespace NMultiTool.Library.Module.Commands.CreateNugetSpec
{
    public interface INuGetSpec
    {
        void Create(string nugetManifestTemplate, string nugetmanifestTarget, string assemblyPath);
    }
}

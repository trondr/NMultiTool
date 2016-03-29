using System.Collections.Generic;

namespace NMultiTool.Library.Module.Commands.InstallUtil
{
    public interface IInstallUtil
    {
        int Execute(InstallAction installAction, string directory, List<string> includeFileSpecs, List<string> excludeFileSpecs);
    }
}

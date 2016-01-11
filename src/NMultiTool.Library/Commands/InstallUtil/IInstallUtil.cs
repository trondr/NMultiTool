using System.Collections.Generic;

namespace NMultiTool.Library.Commands.InstallUtil
{
    public interface IInstallUtil
    {
        int Execute(InstallAction installAction, string directory, List<string> includeFileSpecs, List<string> excludeFileSpecs);
    }
}

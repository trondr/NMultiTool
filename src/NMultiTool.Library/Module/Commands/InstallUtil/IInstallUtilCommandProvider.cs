using System.Collections.Generic;
using NCmdLiner;

namespace NMultiTool.Library.Module.Commands.InstallUtil
{
    public interface IInstallUtilCommandProvider
    {
        Result<int> InstallUtil(InstallAction installAction, string directory, List<string> includeFileSpecs, List<string> excludeFileSpecs);
    }
}
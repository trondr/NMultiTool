using System.Collections.Generic;
using NCmdLiner;

namespace NMultiTool.Library.Module.Commands.InstallUtil
{
    public class InstallUtilCommandProvider : IInstallUtilCommandProvider
    {
        private readonly IInstallUtil _installUtil;

        public InstallUtilCommandProvider(IInstallUtil installUtil)
        {
            _installUtil = installUtil;
        }

        public Result<int> InstallUtil(InstallAction installAction, string directory, List<string> includeFileSpecs, List<string> excludeFileSpecs)
        {
            return Result.Ok(_installUtil.Execute(installAction, directory, includeFileSpecs, excludeFileSpecs));
        }
    }
}
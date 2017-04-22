using System;
using Common.Logging;
using NCmdLiner.Attributes;
using NMultiTool.Library.Infrastructure;
using NMultiTool.Library.Module.Commands.Folder2Wxs;

namespace NMultiTool.Module.Commands
{
    public class Folder2WxsCommand: CommandDefinition
    {
        private readonly IFolder2Wxs _folder2Wxs;
        private readonly ILog _logger;

        public Folder2WxsCommand(IFolder2Wxs folder2Wxs,ILog logger)
        {
            _folder2Wxs = folder2Wxs;
            _logger = logger;
        }

        [CLSCompliant(false)]
        [Command(Description = "Harvest files and sub folders into a Wxs file for inclusion into a WiX setup project."
            )]
        public int Folder2Wxs
            (
                [RequiredCommandParameter(
                Description = "Path to harvest folder. All files and sub folders in this folder will be harvested",
                ExampleValue = "c:\\temp\\SomeSourceDirectory",
                AlternativeName = "d"
                )] string harvestFolder,
            [RequiredCommandParameter(
                Description = "Output wsx file. The wxs file to be included in a WiX setup project.",
                ExampleValue = "c:\\temp\\MySetupProject\\Components.wxs",
                AlternativeName = "wf"
                )] string wxsFileName,
            [RequiredCommandParameter(
                Description = "Target folder id. The target folder id represents the folder location where the files and sub directories will be installed by the MSI file compiled from the resulting wxs file.",
                ExampleValue = "TargetFolder",
                AlternativeName = "ti"
                )] string targetFolderId,
            [RequiredCommandParameter(
                Description = "Component group id. The files and sub folders in the harvest folder will be grouped as components underneath this component group id.",
                ExampleValue = "Components_WixComponentGroup",
                AlternativeName = "ci"
                )] string componentGroupId,
            [RequiredCommandParameter(
                Description = "Disk ids to distribute files to. The files will be evenly stored in different cabinet files corresponding to the disk id. Typically specify more than one disk id if the total size of the subfolder is larger than 2 GB. Each disk id can store approx 2 GB. If folder is less than 2 GB one disk is enough.",
                ExampleValue = new[] { "1", "2", "3" },
                AlternativeName = "di"
                )] string[] diskIds,
            [RequiredCommandParameter(
                Description = "Specify that any executables in the directory shall be added to apps path when installed by the Wix setup",
                ExampleValue = true,
                AlternativeName = "af"
                )] bool addExecutables2AppsPath,
            [OptionalCommandParameter(
                    Description = "Set KeyPath=Yes on all files harvested. If installing application using install scope 'perUser', enableKeyPath must be set to false.",
                    ExampleValue = true,
                    DefaultValue = true,
                    AlternativeName = "ekp"
                )] bool enableKeyPath,
                [OptionalCommandParameter(
                    Description = "Company name to use in HKCU registry setting if enabledKeyPath is set to False",
                    ExampleValue = "MyCompany",
                    DefaultValue = "",
                    AlternativeName = "co"
                )] string companyName,
                [OptionalCommandParameter(
                    Description = "Application name to use in HKCU registry setting if enabledKeyPath is set to False",
                    ExampleValue = "MyApplication",
                    DefaultValue = "",
                    AlternativeName = "an"
                )] string applicationName
            )
        {
            var returnValue = 0;
            _logger.Info("Start: Folder2Wxs");            
            _folder2Wxs.Harvest(harvestFolder,wxsFileName,targetFolderId,componentGroupId,diskIds,addExecutables2AppsPath, enableKeyPath, companyName, applicationName);
            _logger.Info("Stop: Folder2Wxs");
            return returnValue;
        }
    }
}

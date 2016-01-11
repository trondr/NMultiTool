using System;
using NCmdLiner.Attributes;
using NMultiTool.Library.Commands.FindDuplicateFiles;
using NMultiTool.Library.Common;

namespace NMultiTool.Commands
{
    public class FindDuplicateFilesCommand: CommandsBase
    {
        private readonly IFindDuplicateFilesCommandProvider _findDuplicateFilesCommandProvider;

        public FindDuplicateFilesCommand(IFindDuplicateFilesCommandProvider findDuplicateFilesCommandProvider)
        {
            _findDuplicateFilesCommandProvider = findDuplicateFilesCommandProvider;
        }

        [CLSCompliant(false)]
        [Command(Description = "Find duplicate files in one or more locations. Use rules to suggest wich duplicate files to keep and which to delete. The rules defined first have priority over rules defined after. The provided example values will search the 'd:' drive and 'e:' drive for duplicate files and suggest deleting files on the the 'e:' drive since the regular expression for the e drive is listed first.")]
        public int FindDuplicateFiles(
            [RequiredCommandParameter(Description = "Array of paths to search for duplicate files.", ExampleValue = new string[]{@"d:\Dev", @"e:\Dev"}, AlternativeName = "ps")]
                string[] pathsToSearch,
            [RequiredCommandParameter(Description = "Regular expression based rules for wich file locations to keep. Files in locations defined last or not defined  will be marked for deletion. The provided example value instructs to keep files on d: and mark any duplicates on e: for deletion.", ExampleValue = new string[] { "^d:.+", "^e:.+" },AlternativeName = "flk")]string[] fileLocationsToKeep,
            [RequiredCommandParameter(Description = "Result file containing the suggestions for which files to keep.",ExampleValue = @"c:\temp\DeleteDuplicateFiles.cmd.txt", AlternativeName = "rf")]string resultFile
            )
        {                       
            var returnValue = _findDuplicateFilesCommandProvider.FindDuplicateFiles(pathsToSearch, fileLocationsToKeep, resultFile);                     
            return returnValue;
        }
    }

    
}
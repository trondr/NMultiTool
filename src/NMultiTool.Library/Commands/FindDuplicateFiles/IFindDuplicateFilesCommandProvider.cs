﻿namespace NMultiTool.Library.Commands.FindDuplicateFiles
{
    public interface IFindDuplicateFilesCommandProvider
    {
        int FindDuplicateFiles(string[] pathsToSearch, string[] fileLocationsToKeep, string resultFile);
    }
}

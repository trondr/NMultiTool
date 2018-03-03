using System;
using System.IO;
using NCmdLiner;
using NMultiTool.Library.Module.Commands.ConvertSvgToIco;

namespace NMultiTool.Library.Module.Commands.Folder2Wxs
{
    public class FolderPath
    {
        private FolderPath(string folderPath)
        {
            this.Value = folderPath;
        }

        public string Value { get; }

        public static implicit operator FolderPath(string folderPath)
        {
            var result = Create(folderPath)
                .OnFailure(exception => throw exception);
            return result.Value;
        }

        public static Result<FolderPath> Create(string folderPath)
        {
            try
            {
                var fullFolderPath = Path.GetFullPath(folderPath);
                return Result.Ok(new FolderPath(fullFolderPath));
            }
            catch (Exception e)
            {
                return Result.Fail<FolderPath>(new NMultiToolException($"Invalid folder path '{folderPath}'. {e.Message}", e));
            }
        }
    }
}
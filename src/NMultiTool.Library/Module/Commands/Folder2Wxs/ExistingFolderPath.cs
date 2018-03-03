using System;
using System.IO;
using NCmdLiner;
using NMultiTool.Library.Module.Commands.ConvertSvgToIco;

namespace NMultiTool.Library.Module.Commands.Folder2Wxs
{
    public class ExistingFolderPath
    {
        private ExistingFolderPath(string folderPath)
        {
            this.Value = folderPath;
        }

        public string Value { get; }

        public static implicit operator ExistingFolderPath(string folderPath)
        {
            var result = Create(folderPath)
                .OnFailure(exception => throw exception);
            return result.Value;
        }

        public static Result<ExistingFolderPath> Create(string folderPath)
        {
            try
            {
                var fullFolderPath = Path.GetFullPath(folderPath);
                return Directory.Exists(fullFolderPath) ? 
                    Result.Ok(new ExistingFolderPath(fullFolderPath)) : 
                    Result.Fail<ExistingFolderPath>(new DirectoryNotFoundException($"Folder path does not exist: {fullFolderPath}"));
            }
            catch (Exception e)
            {
                return Result.Fail<ExistingFolderPath>(new NMultiToolException($"Invalid folder path '{folderPath}'. {e.Message}", e));
            }
        }
    }
}
using System;

namespace NMultiTool.Library.Module.Commands.Folder2Wxs
{
    public class HarvestInfo
    {
        public ExistingFolderPath Path { get; set; }
        public bool EnableKeyPath { get; set; }
        public string CompanyName { get; set; }
        public string ApplicationName { get; set; }
        public string ComponentGroupId { get; set; }
        public string TargetFolderId { get; set; }
        public string WsxFileName { get; set; }
        public string[] DiskIds { get; set; }
        public bool AddExecutables2AppsPath { get; set; }
        public YesOrNoOrVar IsWin64 { get; set; }
    }
}
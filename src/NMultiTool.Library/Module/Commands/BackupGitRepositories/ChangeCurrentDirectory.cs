using System;
using System.IO;

namespace NMultiTool.Library.Module.Commands.BackupGitRepositories
{
    public class ChangeCurrentDirectory : IDisposable
    {
        private readonly string _previousCurrentDirectory;
        private bool _disposed = false;

        public ChangeCurrentDirectory(string currentDirectory)
        {
            _previousCurrentDirectory = Directory.GetCurrentDirectory();
            Directory.SetCurrentDirectory(currentDirectory);
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                RestorePreviousCurrentDirectory();                
                _disposed = true;
            }
        }

        private void RestorePreviousCurrentDirectory()
        {
            Directory.SetCurrentDirectory(_previousCurrentDirectory);
        }
    }
}
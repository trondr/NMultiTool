using System;
using System.Collections.Generic;
using System.Configuration.Install;
using System.IO;
using Common.Logging;
using NMultiTool.Library.BootStrap;
using NMultiTool.Library.Common;

namespace NMultiTool.Library.Commands.InstallUtil
{
    public class InstallUtil : IInstallUtil
    {
        private readonly ILoggingConfiguration _loggingConfiguration;
        private readonly IFolderFileSearcher _folderFileSearcher;
        private readonly ILog _logger;

        private string SetupLogDirectory
        {
            get
            {
                if (_setupLogDirectory == null)
                {
                    _setupLogDirectory = Path.Combine(_loggingConfiguration.LogDirectoryPath, "Setup");
                    CreateDirectory(new DirectoryInfo(_setupLogDirectory));
                }
                return _setupLogDirectory;
            }
        }
        private static string _setupLogDirectory;

        public InstallUtil(ILoggingConfiguration loggingConfiguration,IFolderFileSearcher folderFileSearcher,ILog logger)
        {
            _loggingConfiguration = loggingConfiguration;
            _folderFileSearcher = folderFileSearcher;
            _logger = logger;
        }

        public int Execute(InstallAction installAction, string directory, List<string> includeFileSpecs, List<string> excludeFileSpecs)
        {
            var returnValue = 0;            
            try
            {
                if (!Directory.Exists(directory)) throw new DirectoryNotFoundException("Directory not found: " + directory);
                if (includeFileSpecs.Count == 0) throw new ArgumentOutOfRangeException("includeFileSpecs", "No files are included");                
                _logger.InfoFormat("Action: {0}, Directory: {1}, Include files: '{2}', Exclude files: '{3}'", installAction, directory, string.Join(";", includeFileSpecs), string.Join(";", excludeFileSpecs));
                var excludedFiles = _folderFileSearcher.GetFiles(directory, excludeFileSpecs);
                var includedFiles = _folderFileSearcher.GetFiles(directory, includeFileSpecs);
                foreach (var includedFile in includedFiles.Values)
                {
                    if (excludedFiles.ContainsKey(includedFile.Name))
                    {
                        _logger.InfoFormat("Excluding from install '{0}'", includedFile.FullName);
                        continue;
                    }

                    if (installAction == InstallAction.UnInstall || installAction == InstallAction.UnInstallInstall)
                    {
                        _logger.InfoFormat("UnInstalling '{0}'...", includedFile.FullName);
                        var arguments = GetUnInstallArguments(includedFile);
                        _logger.InfoFormat("Calling the managed installer with the arguments:{0}{1}", Environment.NewLine, string.Join(" ", arguments));
                        ManagedInstallerClass.InstallHelper(arguments);
                    }

                    if (installAction == InstallAction.Install || installAction == InstallAction.UnInstallInstall)
                    {
                        _logger.InfoFormat("Installing '{0}'...", includedFile.FullName);
                        var arguments = GetInstallArguments(includedFile);
                        _logger.InfoFormat("Calling the managed installer with the arguments:{0}{1}", Environment.NewLine, string.Join(" ", arguments));
                        ManagedInstallerClass.InstallHelper(arguments);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.ErrorFormat("Failed to execute install action '{0}' on directory '{1}'. {2}",installAction, directory, ex.ToString());
                returnValue = 1;
            }
            return returnValue;            
        }

        

        private string[] GetUnInstallArguments(FileInfo file)
        {
            if(file == null) throw new ArgumentNullException("file","file is null");
            if (file.Directory == null) throw new ArgumentNullException("file", "file.Directory is null");
            if(!file.Exists) throw new FileNotFoundException("File not found",file.FullName);
            var arguments = new string[5];
            arguments[0] = "/u";
            arguments[1] = string.Format("/LogFile={0}", Path.Combine(SetupLogDirectory, file.Name + ".UnInstall.log"));
            var pdbFile = new FileInfo(Path.Combine(file.Directory.FullName, Path.GetFileNameWithoutExtension(file.FullName) + ".pdb"));
            arguments[2] = string.Format("/PdbFile={0}", pdbFile.FullName);
            arguments[3] = string.Format("/DllFile={0}", file.FullName);
            arguments[4] = string.Format("{0}", file.FullName);
            if (File.Exists(file.FullName)) _logger.InfoFormat("Assembly exists: '{0}'", file.FullName);
            else _logger.WarnFormat("Assembly does not exist: '{0}'", file.FullName);
            if (File.Exists(file.FullName)) _logger.InfoFormat("Pdb exists: '{0}'", pdbFile.FullName);
            else _logger.WarnFormat("Pdb does not exist: '{0}'", pdbFile.FullName);            
            return arguments;
        }

        private string[] GetInstallArguments(FileInfo file)
        {
            if (file == null) throw new ArgumentNullException("file", "file is null");
            if (file.Directory == null) throw new ArgumentNullException("file", "file.Directory is null");
            if (!file.Exists) throw new FileNotFoundException("File not found", file.FullName);
            var arguments = new string[4];
            arguments[0] = string.Format("/LogFile={0}", Path.Combine(SetupLogDirectory, file.Name + ".Install.log"));
            var pdbFile = new FileInfo(Path.Combine(file.Directory.FullName, Path.GetFileNameWithoutExtension(file.FullName) + ".pdb"));
            arguments[1] = string.Format("/PdbFile={0}", pdbFile.FullName);
            arguments[2] = string.Format("/DllFile={0}", file.FullName);
            arguments[3] = string.Format("{0}", file.FullName);

            if (File.Exists(file.FullName)) _logger.InfoFormat("Assembly exists: '{0}'", file.FullName);
            else _logger.WarnFormat("Assembly does not exist: '{0}'", file.FullName);
            if (File.Exists(file.FullName)) _logger.InfoFormat("Pdb exists: '{0}'", pdbFile.FullName);
            else _logger.WarnFormat("Pdb does not exist: '{0}'", pdbFile.FullName);
            return arguments;
        }

        private static void CreateDirectory(DirectoryInfo directoryinfo)
        {
            if (!directoryinfo.Exists)
            {
                CreateDirectory(directoryinfo.Parent);
                directoryinfo.Create();
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using Common.Logging;

namespace NMultiTool.Library.Module.Commands.FindDuplicateFiles
{
    public class FindDuplicateFilesCommandProvider : IFindDuplicateFilesCommandProvider
    {
        private readonly ILog _logger;

        public FindDuplicateFilesCommandProvider(ILog logger)
        {
            _logger = logger;
        }

        public int FindDuplicateFiles(string[] pathsToSearch, string[] fileLocationsToKeep, string resultFile)
        {
            resultFile = Environment.ExpandEnvironmentVariables(resultFile);
            var returnCode = 0;
            _logger.Info("Checking if result file can be created");
            CheckIfResultFileCanBeCreated(resultFile);

            _logger.Info("Finding files with the same file length");
            var filesOfEqualLength = FindAllFilesAndGroupByFileLength(pathsToSearch);

            _logger.Info("Foreach file with the same file file length calculate the the first 1000 bytes sha1 hash.");
            var duplicateFiles1 = FindPossibleDuplicateFiles(filesOfEqualLength);

            _logger.Info("Foreach file with the same 1000 byte sha1 hash, calculate the full sha1 hash.");
            var duplicateFiles = ConfirmDuplicateFiles(duplicateFiles1);

            if (_logger.IsInfoEnabled) _logger.Info("For each file with the same sha1 checksum, sort according to the rules and suggest what file to delete.");
            ReportAndSuggestWhatDuplicateFilesToDelete(duplicateFiles, fileLocationsToKeep, resultFile);

            return returnCode;
        }

        private static void CheckIfResultFileCanBeCreated(string resultFile)
        {
            if(File.Exists(resultFile))
            {
                throw new Exception("Result file allready exists: " + resultFile);
            }            
            using (var sw = new StreamWriter(resultFile))
            {
            }
        }

        private Dictionary<long, List<DuplicateFileInfo>> FindAllFilesAndGroupByFileLength(string[] pathsToSearch)
        {
            var filesOfEqualLength = new Dictionary<long, List<DuplicateFileInfo>>();
            foreach (string path in pathsToSearch)
            {
                if (_logger.IsInfoEnabled) _logger.InfoFormat("Searching {0}...", path);
                var allFileNames = Directory.EnumerateFiles(path, "*.*", SearchOption.AllDirectories);
                foreach (var fileName in allFileNames)
                {
                    var file = new FileInfo(fileName);
                    var length = file.Length;
                    if (filesOfEqualLength.ContainsKey(length))
                    {
                        Console.Write("+"); //Adding extra
                        filesOfEqualLength[length].Add(new DuplicateFileInfo(file));
                    }
                    else
                    {
                        Console.Write("*"); //Adding new
                        var lengthFiles = new List<DuplicateFileInfo> {new DuplicateFileInfo(file)};
                        filesOfEqualLength.Add(length, lengthFiles);
                    }
                }
                Console.WriteLine();
            }
            return filesOfEqualLength;
        }

        private Dictionary<string, List<DuplicateFileInfo>> FindPossibleDuplicateFiles(Dictionary<long, List<DuplicateFileInfo>> filesOfEqualLength)
        {
            var duplicateFiles1 = new Dictionary<string, List<DuplicateFileInfo>>();
            foreach (var files in filesOfEqualLength.Values)
            {
                if (files.Count > 1)
                {
                    foreach (var file in files)
                    {
                        if (!File.Exists(file.FullName)) continue;
                        if (_logger.IsTraceEnabled) _logger.TraceFormat("{0};{1}", file.Length, file.FullName);
                        file.Sha1Mini = Sha1HashOfTheFirst1000Bytes(file.FullName);
                        if (duplicateFiles1.ContainsKey(file.Sha1Mini))
                        {
                            Console.Write("+"); //Adding duplicate
                            duplicateFiles1[file.Sha1Mini].Add(file);
                        }
                        else
                        {
                            Console.Write("*"); //Adding new
                            var checksumFiles = new List<DuplicateFileInfo>();
                            checksumFiles.Add(file);
                            duplicateFiles1.Add(file.Sha1Mini, checksumFiles);
                        }
                    }
                }
            }
            return duplicateFiles1;
        }

        private Dictionary<string, List<DuplicateFileInfo>> ConfirmDuplicateFiles(Dictionary<string, List<DuplicateFileInfo>> duplicateFiles1)
        {
            var duplicateFiles = new Dictionary<string, List<DuplicateFileInfo>>();
            foreach (var files in duplicateFiles1.Values)
            {
                if (files.Count > 1)
                {
                    foreach (var file in files)
                    {
                        if (!File.Exists(file.FullName)) continue;
                        if (_logger.IsTraceEnabled)
                            _logger.TraceFormat("{0};{1};{2}", file.Sha1Mini, file.Length, file.FullName);
                        file.Sha1 = Sha1Hash(file.FullName);
                        if (duplicateFiles.ContainsKey(file.Sha1))
                        {
                            Console.Write("+"); //Adding duplicate
                            duplicateFiles[file.Sha1].Add(file);
                        }
                        else
                        {
                            Console.Write("*"); //Adding new
                            var checksumFiles = new List<DuplicateFileInfo>();
                            checksumFiles.Add(file);
                            duplicateFiles.Add(file.Sha1, checksumFiles);
                        }
                    }
                }
            }
            return duplicateFiles;
        }

        private void ReportAndSuggestWhatDuplicateFilesToDelete(Dictionary<string, List<DuplicateFileInfo>> duplicateFiles, string[] fileLocationsToKeep, string resultFile)
        {
            var duplicateFilesCompare = new DuplicateFilesCompare(fileLocationsToKeep);
            using (var sw = new StreamWriter(resultFile))
            {
                foreach (var files in duplicateFiles.Values)
                {
                    if (files.Count > 1)
                    {
                        files.Sort(duplicateFilesCompare.ComparePaths);
                        for (int i = 0; i < files.Count; i++)
                        {
                            if (i == 0)
                            {
                                sw.WriteLine("REM \"{0}\"", files[i]); //The one to keep
                            }
                            else
                            {
                                sw.WriteLine("del \"{0}\"", files[i]); //The ones to delete
                            }
                        }
                    }
                }
            }
        }

        private static string Sha1HashOfTheFirst1000Bytes(string file)
        {
            byte[] buffer;
            using (Stream stream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                int bufferSize;
                buffer = GetBuffer(stream.Length, out bufferSize);
                stream.Read(buffer, 0, bufferSize);
                stream.Close();
            }
            var sha1 = System.Security.Cryptography.SHA1.Create();
            var hash = BitConverter.ToString(sha1.ComputeHash(buffer)).Replace("-", String.Empty).ToLower();
            return hash;
        }

        private static byte[] GetBuffer(long length, out int bufferSize)
        {
            if (length < 1000)
            {
                bufferSize = (int)length;
                return new byte[length];
            }
            bufferSize = 1000;
            return new byte[1000];
        }

        private static string Sha1Hash(string fileToHash)
        {
            string hash;
            using (Stream stream = new FileStream(fileToHash, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var sha1 = System.Security.Cryptography.SHA1.Create();
                hash = BitConverter.ToString(sha1.ComputeHash(stream)).Replace("-", String.Empty).ToLower();
                stream.Close();
            }
            return hash;
        }
    }
}
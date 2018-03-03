using System;
using System.IO;
using System.Xml;
using Common.Logging;
using NMultiTool.Library.Module.Common.Xml;

namespace NMultiTool.Library.Module.Commands.Folder2Wxs
{
    public class Folder2Wxs : IFolder2Wxs
    {
        private readonly IIdFromNameGenerator _idFromNameGenerator;
        private readonly IXmlHelper _xmlHelper;
        private readonly ILog _logger;

        public Folder2Wxs(IIdFromNameGenerator idFromNameGenerator,IXmlHelper xmlHelper, ILog logger)
        {
            _idFromNameGenerator = idFromNameGenerator;
            _xmlHelper = xmlHelper;
            _logger = logger;
        }
        
        public void Harvest(HarvestInfo harvestInfo)
        {
            if (!harvestInfo.EnableKeyPath)
            {
                if (string.IsNullOrWhiteSpace(harvestInfo.CompanyName))
                    throw new ArgumentException("/companyName must be specified on the command line.");
                if (string.IsNullOrWhiteSpace(harvestInfo.ApplicationName))
                    throw new ArgumentException("/applicationName must be specified on the command line.");
            }

            if (_logger.IsInfoEnabled) _logger.InfoFormat("Harvesting to wsx from directory '{0}'...", harvestInfo.Path.Value);
            var wsxXmlDocument = new XmlDocument();
            var xmlDeclaration = wsxXmlDocument.CreateXmlDeclaration("1.0", "utf-8", String.Empty);
            wsxXmlDocument.AppendChild(xmlDeclaration);

            wsxXmlDocument.AppendChild(wsxXmlDocument.CreateProcessingInstruction("if", $"$(var.Platform) = x64"));
            wsxXmlDocument.AppendChild(wsxXmlDocument.CreateProcessingInstruction("define", $"bitness = \"(64 bit)\""));
            wsxXmlDocument.AppendChild(wsxXmlDocument.CreateProcessingInstruction("define", $"Win64 = \"yes\""));
            wsxXmlDocument.AppendChild(wsxXmlDocument.CreateProcessingInstruction("else", string.Empty));
            wsxXmlDocument.AppendChild(wsxXmlDocument.CreateProcessingInstruction("define", $"bitness = \"(32 bit)\""));
            wsxXmlDocument.AppendChild(wsxXmlDocument.CreateProcessingInstruction("define", $"Win64 = \"no\""));
            wsxXmlDocument.AppendChild(wsxXmlDocument.CreateProcessingInstruction("endif", string.Empty));

            var wsxRootElement = wsxXmlDocument.CreateElement("Wix");
            wsxXmlDocument.AppendChild(wsxRootElement);
            _xmlHelper.SetAttribute(wsxXmlDocument.DocumentElement, "xmlns", "http://schemas.microsoft.com/wix/2006/wi");
            if (wsxXmlDocument.DocumentElement == null) throw new XmlException("xmldocument document element is null");
            var fragment1 = wsxXmlDocument.CreateElement("Fragment");
            if (wsxXmlDocument.DocumentElement == null) throw new XmlException("xmldocument document element is null");
            wsxXmlDocument.DocumentElement.AppendChild(fragment1);
            var fragment2 = wsxXmlDocument.CreateElement("Fragment");
            if (wsxXmlDocument.DocumentElement == null) throw new XmlException("xmldocument document element is null");
            wsxXmlDocument.DocumentElement.AppendChild(fragment2);
            var componentGroupXmlNode = wsxXmlDocument.CreateElement("ComponentGroup");
            _xmlHelper.SetAttribute(componentGroupXmlNode, "Id", harvestInfo.ComponentGroupId);
            fragment2.AppendChild(componentGroupXmlNode);
            var directoryRefElement = wsxXmlDocument.CreateElement("DirectoryRef");
            _xmlHelper.SetAttribute(directoryRefElement, "Id", harvestInfo.TargetFolderId);
            fragment1.AppendChild(directoryRefElement);
            AddDirectory(new DirectoryInfo(harvestInfo.Path.Value), directoryRefElement, componentGroupXmlNode, harvestInfo.TargetFolderId, harvestInfo.DiskIds, harvestInfo.AddExecutables2AppsPath, harvestInfo.EnableKeyPath, harvestInfo.CompanyName, harvestInfo.ApplicationName,harvestInfo.IsWin64.Value);
            _xmlHelper.SaveDocument(wsxXmlDocument, harvestInfo.WsxFileName);
            if (_logger.IsInfoEnabled) _logger.InfoFormat("Finished harvesting to wsx from directory '{0}'...", harvestInfo.Path.Value);
        }

        #region Private methods
        private void AddDirectory(DirectoryInfo directory, 
            XmlElement directoryRefElement, 
            XmlElement componentGroupElement, 
            string targetFolderId,
            string[] diskIds, 
            bool addExesToAppsPath, 
            bool enableKeyPath, 
            string companyName, 
            string applicationName,
            string isWin64)
        {            
            var files = directory.GetFiles();
            foreach (var fileInfo in files)
            {
                if (directoryRefElement.OwnerDocument != null)
                {
                    //ComponentRef
                    var componentRefElement = directoryRefElement.OwnerDocument.CreateElement("ComponentRef");
                    var componentId = _idFromNameGenerator.GetId(fileInfo.Name, "WixComponent");
                    _xmlHelper.SetAttribute(componentRefElement, "Id", componentId);
                    componentGroupElement.AppendChild(componentRefElement);

                    //Component
                    var componentElement = directoryRefElement.OwnerDocument.CreateElement("Component");
                    _xmlHelper.SetAttribute(componentElement, "Id", componentId);
                    _xmlHelper.SetAttribute(componentElement, "Guid", Guid.NewGuid().ToString().ToUpper());
                    _xmlHelper.SetAttribute(componentElement, "DiskId", GetRandomDiskId(diskIds));
                    _xmlHelper.SetAttribute(componentElement, "Win64", isWin64);
                    directoryRefElement.AppendChild(componentElement);
                    if (directoryRefElement.OwnerDocument == null) throw new ArgumentNullException("directoryRefElement", "directoryRefElement.OwnerDocument is null.");
                    var createFolderElement = directoryRefElement.OwnerDocument.CreateElement("CreateFolder");
                    componentElement.AppendChild(createFolderElement);

                    var removeFolderElement = directoryRefElement.OwnerDocument.CreateElement("RemoveFolder");
                    _xmlHelper.SetAttribute(removeFolderElement, "Id", _idFromNameGenerator.GetId(directory.Name, "WixRemoveFolder"));
                    _xmlHelper.SetAttribute(removeFolderElement, "On", "uninstall");
                    componentElement.AppendChild(removeFolderElement);

                    var removeFileElement = directoryRefElement.OwnerDocument.CreateElement("RemoveFile");
                    _xmlHelper.SetAttribute(removeFileElement, "Id", _idFromNameGenerator.GetId(directory.Name, "WixRemoveFile"));
                    _xmlHelper.SetAttribute(removeFileElement, "Name", "*.*");
                    _xmlHelper.SetAttribute(removeFileElement, "On", "uninstall");
                    componentElement.AppendChild(removeFileElement);

                    var fileElement = directoryRefElement.OwnerDocument.CreateElement("File");
                    var fileId = _idFromNameGenerator.GetId(fileInfo.Name, "WixFile");
                    _xmlHelper.SetAttribute(fileElement, "Id", fileId);
                    _xmlHelper.SetAttribute(fileElement, "Name", fileInfo.Name);
                    if (enableKeyPath)
                    {
                        _xmlHelper.SetAttribute(fileElement, "KeyPath", "yes");
                    }
                    else
                    {
                        _xmlHelper.SetAttribute(fileElement, "KeyPath", "no");
                        var registryKeyElement = directoryRefElement.OwnerDocument.CreateElement("RegistryKey");
                        _xmlHelper.SetAttribute(registryKeyElement, "Root", "HKCU");
                        _xmlHelper.SetAttribute(registryKeyElement, "Key", $"Software\\{companyName}\\{applicationName}");                        
                        componentElement.AppendChild(registryKeyElement);
                        var registryValueElement = directoryRefElement.OwnerDocument.CreateElement("RegistryValue");
                        _xmlHelper.SetAttribute(registryValueElement, "Name", "Version");
                        _xmlHelper.SetAttribute(registryValueElement, "Value", "[ProductVersion]");
                        _xmlHelper.SetAttribute(registryValueElement, "Type", "string");
                        _xmlHelper.SetAttribute(registryValueElement, "KeyPath", "yes");
                        registryKeyElement.AppendChild(registryValueElement);
                    }
                    _xmlHelper.SetAttribute(fileElement, "Source", fileInfo.FullName);
                    componentElement.AppendChild(fileElement);

                    var fileExtension = Path.GetExtension(fileInfo.FullName);
                    if (!string.IsNullOrEmpty(fileExtension) && fileExtension.ToLower() == ".exe" && addExesToAppsPath)
                    {
                        //<RegistryKey Root="HKLM" Key="SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\SomeExeFile.exe" Action="createAndRemoveOnUninstall"/>
                        var registryKeyElement = directoryRefElement.OwnerDocument.CreateElement("RegistryKey");
                        _xmlHelper.SetAttribute(registryKeyElement, "Root", "HKLM");
                        _xmlHelper.SetAttribute(registryKeyElement, "Key", "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\App Paths\\" + fileInfo.Name);
                        //SetAttribute(registryKeyElement, "Action", "createAndRemoveOnUninstall");
                        componentElement.AppendChild(registryKeyElement);

                        //<RegistryValue Root="HKLM" Key="SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\SomeExeFile.exe" Value="[#SomeExeFile_exe_WiXFile]" Type="string" Action="write"/>
                        var registryValueElement1 = directoryRefElement.OwnerDocument.CreateElement("RegistryValue");
                        _xmlHelper.SetAttribute(registryValueElement1, "Root", "HKLM");
                        _xmlHelper.SetAttribute(registryValueElement1, "Key", "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\App Paths\\" + fileInfo.Name);
                        _xmlHelper.SetAttribute(registryValueElement1, "Value", string.Format("[#{0}]", fileId));
                        _xmlHelper.SetAttribute(registryValueElement1, "Type", "string");
                        _xmlHelper.SetAttribute(registryValueElement1, "Action", "write");
                        componentElement.AppendChild(registryValueElement1);

                        //<RegistryValue Root="HKLM" Key="SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\SomeExeFile.exe" Name="path" Value="[TargetPathFolder]" Type="string" Action="write"/>
                        var registryValueElement2 = directoryRefElement.OwnerDocument.CreateElement("RegistryValue");
                        _xmlHelper.SetAttribute(registryValueElement2, "Root", "HKLM");
                        _xmlHelper.SetAttribute(registryValueElement2, "Key", "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\App Paths\\" + fileInfo.Name);
                        _xmlHelper.SetAttribute(registryValueElement2, "Name", "path");
                        _xmlHelper.SetAttribute(registryValueElement2, "Value", string.Format("[{0}]", targetFolderId));
                        _xmlHelper.SetAttribute(registryValueElement2, "Type", "string");
                        _xmlHelper.SetAttribute(registryValueElement2, "Action", "write");
                        componentElement.AppendChild(registryValueElement2);
                    }
                }
                else
                {
                    if (_logger.IsWarnEnabled) _logger.Warn("directoryRefElement.OwnerDocument is null");
                }
            }
            var directories = directory.GetDirectories();
            foreach (var fileInfo in directories)
            {
                if (fileInfo.Name != ".svn" && fileInfo.Name != ".hg" && fileInfo.Name != ".git") // ignore source control directories
                {
                    if (directoryRefElement.OwnerDocument != null)
                    {
                        var directoryXmlElement = directoryRefElement.OwnerDocument.CreateElement("Directory");
                        var directoryId = _idFromNameGenerator.GetId(fileInfo.Name, "WixDirectory");
                        _xmlHelper.SetAttribute(directoryXmlElement, "Id", directoryId);
                        _xmlHelper.SetAttribute(directoryXmlElement, "Name", fileInfo.Name);
                        directoryRefElement.AppendChild(directoryXmlElement);
                        AddDirectory(fileInfo, directoryXmlElement, componentGroupElement, targetFolderId, diskIds, addExesToAppsPath, enableKeyPath, companyName, applicationName,isWin64);
                    }
                    else
                    {
                        if (_logger.IsWarnEnabled) _logger.Warn("directoryRefElement.OwnerDocument is null");
                    }
                }
                else
                {
                    if (_logger.IsDebugEnabled) _logger.DebugFormat("Ignoring : " + fileInfo.FullName);
                }
            }
        }

        private readonly Random _randomNumber = new Random(DateTime.Now.Millisecond);
        
        private string GetRandomDiskId(string[] diskIds)
        {
            return diskIds[_randomNumber.Next(1, diskIds.Length + 1) - 1];
        }
        #endregion
    }
}
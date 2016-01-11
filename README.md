NMultiTool
==========

NMultiTool provides various command line commands

## Usage

```
NMultiTool 1.0.14274.243cb7b - NMultiTool provides commands useful during build or install of an application or library.
Copyright © <github.com/trondr> 2013-2014
Author: <github.com\trondr>
Usage: NMultiTool.exe <command> [parameters]

Commands:
---------
Help                      Display this help text
License                   Display license
Credits                   Display credits
CreateNugetSpec           Create nuget spec from a template and update
                          information in the resulting nuget file with meta
                          data from a specified assembly.
Folder2Wxs                Harvest files and sub folders into a Wxs file for
                          inclusion into a WiX setup project.

Commands and parameters:
------------------------
CreateNugetSpec           Create nuget spec from a template and update
                          information in the resulting nuget file with meta
                          data from a specified assembly.
   /manifestTemplate      [Required] The nuget manifest template file.
                          Alternative parameter name: /nmt
   /manifestTarget        [Required] The nuget manifest target file.
                          Alternative parameter name: /tf
   /assemblyPath          [Required] The assembly with meta data to write
                          into the nuget manifest. Alternative parameter
                          name: /ap

   Example: NMultiTool.exe CreateNugetSpec /manifestTemplate="c:\temp\SomeSource.nuspec" /manifestTarget="c:\temp\SomeTarget.nuspec" /assemblyPath="c:\temp\SomeLib.dll" 
   Example (alternative): NMultiTool.exe CreateNugetSpec /nmt="c:\temp\SomeSource.nuspec" /tf="c:\temp\SomeTarget.nuspec" /ap="c:\temp\SomeLib.dll" 


Folder2Wxs                Harvest files and sub folders into a Wxs file for
                          inclusion into a WiX setup project.
   /harvestFolder         [Required] Path to harvest folder. All files and
                          sub folders in this folder will be harvested
                          Alternative parameter name: /d
   /wxsFileName           [Required] Output wsx file. The wxs file to be
                          included in a WiX setup project. Alternative
                          parameter name: /wf
   /targetFolderId        [Required] Target folder id. The target folder id
                          represents the folder location where the files and
                          sub directories will be installed by the MSI file
                          compiled from the resulting wxs file. Alternative
                          parameter name: /ti
   /componentGroupId      [Required] Component group id. The files and sub
                          folders in the harvest folder will be grouped as
                          components underneath this component group id.
                          Alternative parameter name: /ci
   /diskIds               [Required] Disk ids to distribute files to. The
                          files will be evenly stored in different cabinet
                          files corresponding to the disk id. Typically
                          specify more than one disk id if the total size of
                          the subfolder is larger than 2 GB. Each disk id can
                          store approx 2 GB. If folder is less than 2 GB one
                          disk is enough. Alternative parameter name: /di
   /addExecutables2AppsPath[Required] Specify that any executables in the
                          directory shall be added to apps path when
                          installed by the Wix setup Alternative parameter
                          name: /af

   Example: NMultiTool.exe Folder2Wxs /harvestFolder="c:\temp\SomeSourceDirectory" /wxsFileName="c:\temp\MySetupProject\Components.wxs" /targetFolderId="TargetFolder" /componentGroupId="Components_WixComponentGroup" /diskIds="['1';'2';'3']" /addExecutables2AppsPath="True" 
   Example (alternative): NMultiTool.exe Folder2Wxs /d="c:\temp\SomeSourceDirectory" /wf="c:\temp\MySetupProject\Components.wxs" /ti="TargetFolder" /ci="Components_WixComponentGroup" /di="['1';'2';'3']" /af="True"
```
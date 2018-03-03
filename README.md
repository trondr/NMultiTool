# NMultiTool
=========================
NMultiTool provides commands useful during build or install of an application or library

##Usage

```
NMultiTool 1.0.18062.53.9c4d7e2 - NMultiTool provides commands useful during build or install of an application or library
Copyright © github.trondr 2018
Author: trondr@outlook.com
Usage: NMultiTool.exe <command> [parameters]

Commands:
---------
Help                      Display this help text
License                   Display license
Credits                   Display credits
BackupGitRepositories     Backup git repositories.
ConvertAllSvgToIco        Convert svg files in folder (and subfolders if
                          recursive==true) to multi size icon files of
                          specified sizes.
ConvertSvgToIco           Convert a .svg vector graphics file to a multi size
                          .ico file.
CreateNugetSpec           Create nuget spec from a template and update
                          information in the resulting nuget file with meta
                          data from a specified assembly.
ExtractIcon               Extract icons from file.
FindDuplicateFiles        Find duplicate files in one or more locations. 
Folder2Wxs                Harvest files and sub folders into a Wxs file for
                          inclusion into a WiX setup project.
InstallUtil               InstallUtil processes specified .NET assemblies and
                          executes Install or Uninstall on the installer
                          classes defined in the assemblies.
SplitIco                  Split an icon file into multiple images

Commands and parameters:
------------------------
BackupGitRepositories     Backup git repositories.
   /configurationFile     [Required] Path to configuration file Alternative
                          parameter name: /cf

   Example: NMultiTool.exe BackupGitRepositories /configurationFile="c:\temp\BackupGitRepositories.config" 
   Example (alternative): NMultiTool.exe BackupGitRepositories /cf="c:\temp\BackupGitRepositories.config" 


ConvertAllSvgToIco        Convert svg files in folder (and subfolders if
                          recursive==true) to multi size icon files of
                          specified sizes.
   /folder                [Required] Folder to search for svg files
                          Alternative parameter name: /d
   /recursive             [Optional] Recurse sub folders. Alternative
                          parameter name: /r. Default value: True
   /refresh               [Optional] Recreate icon even if it allready
                          exists. Alternative parameter name: /rf. Default
                          value: False
   /sizes                 [Optional] Create multi size icon with the
                          specified array of sizes Alternative parameter
                          name: /s. Default value: [16;32;64;128;256]
   /maxDegreeOfParallelism[Optional] Run specified number of conversions in
                          parallel. Alternative parameter name: /p. Default
                          value: 4

   Example: NMultiTool.exe ConvertAllSvgToIco /folder="c:\temp\icons" /recursive="True" /refresh="False" /sizes="[16;32;64;128;256]" /maxDegreeOfParallelism="4" 
   Example (alternative): NMultiTool.exe ConvertAllSvgToIco /d="c:\temp\icons" /r="True" /rf="False" /s="[16;32;64;128;256]" /p="4" 


ConvertSvgToIco           Convert a .svg vector graphics file to a multi size
                          .ico file.
   /svgFileName           [Required] Path to vector graphics file having
                          format *.svg Alternative parameter name: /svg
   /refresh               [Optional] Recreate icon even if it allready
                          exists. Alternative parameter name: /rf. Default
                          value: False
   /sizes                 [Optional] Sizes of the images included in the
                          multi size .ico file. Alternative parameter name:
                          /s. Default value: [16;32;64;128;256]

   Example: NMultiTool.exe ConvertSvgToIco /svgFileName="c:\temp\test.svg" /refresh="False" /sizes="[16;32;64;128;256]" 
   Example (alternative): NMultiTool.exe ConvertSvgToIco /svg="c:\temp\test.svg" /rf="False" /s="[16;32;64;128;256]" 


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


ExtractIcon               Extract icons from file.
   /sourceFile            [Required] Binary file to extract icons from.
                          Alternative parameter name: /sf
   /targetFolder          [Required] Save icon to this folder. Alternative
                          parameter name: /tf

   Example: NMultiTool.exe ExtractIcon /sourceFile="c:\windows\notepad.exe" /targetFolder="c:\temp" 
   Example (alternative): NMultiTool.exe ExtractIcon /sf="c:\windows\notepad.exe" /tf="c:\temp" 


FindDuplicateFiles        Find duplicate files in one or more locations. Use
                          rules to suggest wich duplicate files to keep and
                          which to delete. The rules defined first have
                          priority over rules defined after. The provided
                          example values will search the 'd:' drive and 'e:'
                          drive for duplicate files and suggest deleting
                          files on the the 'e:' drive since the regular
                          expression for the e drive is listed first.
   /pathsToSearch         [Required] Array of paths to search for duplicate
                          files. Alternative parameter name: /ps
   /fileLocationsToKeep   [Required] Regular expression based rules for wich
                          file locations to keep. Files in locations defined
                          last or not defined will be marked for deletion.
                          The provided example value instructs to keep files
                          on d: and mark any duplicates on e: for deletion.
                          Alternative parameter name: /flk
   /resultFile            [Required] Result file containing the suggestions
                          for which files to keep. Alternative parameter
                          name: /rf

   Example: NMultiTool.exe FindDuplicateFiles /pathsToSearch="['d:\Dev';'e:\Dev']" /fileLocationsToKeep="['^d:.+';'^e:.+']" /resultFile="c:\temp\DeleteDuplicateFiles.cmd.txt" 
   Example (alternative): NMultiTool.exe FindDuplicateFiles /ps="['d:\Dev';'e:\Dev']" /flk="['^d:.+';'^e:.+']" /rf="c:\temp\DeleteDuplicateFiles.cmd.txt" 


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
   /enableKeyPath         [Optional] Set KeyPath=Yes on all files harvested.
                          If installing application using install scope
                          'perUser', enableKeyPath must be set to false.
                          Alternative parameter name: /ekp. Default value:
                          True
   /companyName           [Optional] Company name to use in HKCU registry
                          setting if enabledKeyPath is set to False
                          Alternative parameter name: /co. Default value:
   /applicationName       [Optional] Application name to use in HKCU registry
                          setting if enabledKeyPath is set to False
                          Alternative parameter name: /an. Default value:
   /isWin64               [Optional] Indicate if wix components should be
                          marked with Win64 attribute. Valid values are 'yes'
                          or 'no' or 'var.Win64'. Alternative parameter name:
                          /is64. Default value: no

   Example: NMultiTool.exe Folder2Wxs /harvestFolder="c:\temp\SomeSourceDirectory" /wxsFileName="c:\temp\MySetupProject\Components.wxs" /targetFolderId="TargetFolder" /componentGroupId="Components_WixComponentGroup" /diskIds="['1';'2';'3']" /addExecutables2AppsPath="True" /enableKeyPath="True" /companyName="MyCompany" /applicationName="MyApplication" /isWin64="yes" 
   Example (alternative): NMultiTool.exe Folder2Wxs /d="c:\temp\SomeSourceDirectory" /wf="c:\temp\MySetupProject\Components.wxs" /ti="TargetFolder" /ci="Components_WixComponentGroup" /di="['1';'2';'3']" /af="True" /ekp="True" /co="MyCompany" /an="MyApplication" /is64="yes" 


InstallUtil               InstallUtil processes specified .NET assemblies and
                          executes Install or Uninstall on the installer
                          classes defined in the assemblies.
   /installAction         [Required] Install action. Valid values are
                          Install|UnInstall Alternative parameter name: /a
   /directory             [Required] Directory with .NET components to
                          process. Alternative parameter name: /d
   /includeFileSpecs      [Required] Array of file specifications to include,
                          i.e all files matching the include file
                          specfication will be processed. Alternative
                          parameter name: /if
   /excludeFileSpecs      [Required] Array of file specifications to exclude,
                          i.e all files matching the exclude file
                          specfication will not be processed. Alternative
                          parameter name: /ef

   Example: NMultiTool.exe InstallUtil /installAction="Install" /directory="c:\Program Fils (x86)\SomeCompany\SomeApplication\1.0.0.0" /includeFileSpecs="['SomeCompany.SomeApplication*.dll';'SomeCompany.SomeApplication*.exe']" /excludeFileSpecs="['Microsoft.*.dll';'Microsoft.*.exe']" 
   Example (alternative): NMultiTool.exe InstallUtil /a="Install" /d="c:\Program Fils (x86)\SomeCompany\SomeApplication\1.0.0.0" /if="['SomeCompany.SomeApplication*.dll';'SomeCompany.SomeApplication*.exe']" /ef="['Microsoft.*.dll';'Microsoft.*.exe']" 


SplitIco                  Split an icon file into multiple images
   /icoFileName           [Required] Split ico file in multiple png files
                          Alternative parameter name: /ico

   Example: NMultiTool.exe SplitIco /icoFileName="c:\temp\test.ico" 
   Example (alternative): NMultiTool.exe SplitIco /ico="c:\temp\test.ico" 
 
```

## Minimum Build Requirements

1. Build Tools for Visual Studio 2017 (https://www.visualstudio.com/downloads)
  * Browse down to: Other Tools and Frameworks->Build Tools for Visual Studio 2017
  * Install: vs_buildtools__164170290.1519403159.exe
    * Include: Targeting pack for .NET 4.5.2
    * Include: Nuget targets and build tasks
2. Install latest Wix 3.11 (https://github.com/wixtoolset/wix3/releases/download/wix3111rtm/wix311.exe)
  * On a 32 bit system: Copy "C:\Program Files\MSBuild\Microsoft\WiX" -> "C:\Program Files\Microsoft Visual Studio\2017\BuildTools\MSBuild\Microsoft\WiX"
  * On a 64 bit system: Copy "C:\Program Files (x86)\MSBuild\Microsoft\WiX" -> "C:\Program Files (x86)\Microsoft Visual Studio\2017\BuildTools\MSBuild\Microsoft\WiX"

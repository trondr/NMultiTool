@ECHO OFF
Set ProductName=NMultiTool
Set SolutionName=NMultiTool

IF EXIST "%VSDEVCMD%" goto Build

:VSEnv
SET VSWHEREEXE="%~dp0tools\vswhere\vswhere.exe"
for /f "usebackq tokens=1* delims=: " %%i in (`%VSWHEREEXE% -latest -products * -requires Microsoft.Component.MSBuild`) do (
if /i "%%i"=="installationPath" set dir=%%j
)
Set VSDEVCMDTEST=%dir%\Common7\Tools\VsDevCmd.bat
Echo Checking to see if Visual Studio 2017 is installed ("%VSDEVCMDTEST%")
IF NOT EXIST "%VSDEVCMDTEST%" set BuildMessage="Visual Studio 2017 or later do not seem to be installed (Could not find '%VSDEVCMDTEST%')" & goto Error
Echo Visual Studio 2017 seems to be installed, preparing build environment...
Set VSDEVCMD=%VSDEVCMDTEST%
set VSCMD_START_DIR=%CD%
Call "%VSDEVCMD%"
goto Build

:Build
Echo Building %SolutionName%...
Echo msbuild.exe %SolutionName%.build %1 %2 %3 %4 %5 %6 %7 %8 %9
msbuild.exe %SolutionName%.build %1 %2 %3 %4 %5 %6 %7 %8 %9
Set BuildErrorLevel=%ERRORLEVEL%
IF %BuildErrorLevel%==0 Set BuildMessage=Sucessfully build %ProductName%
IF NOT %BuildErrorLevel% == 0 Set BuildMessage=Failed to build %ProductName%
IF NOT %BuildErrorLevel% == 0 goto Error
Set exitCode=%BuildErrorLevel%
goto End

:Error
Set exitCode=1
goto End

:End
Echo %BuildMessage%  (ExitCode: %exitCode%)
EXIT /B %exitCode%

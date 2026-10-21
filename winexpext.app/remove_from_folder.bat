@echo off

net session 2>>&1 >nul
SET EL=%ERRORLEVEL%
if %EL% NEQ 0 echo This batch file attempts to remove Windows Registry changes for the 
if %EL% NEQ 0 echo application to appear in the Windows Explorer context-menu.
if %EL% NEQ 0 echo This requires the batch file to be run as System Administrator.
if %EL% NEQ 0 echo. 
if %EL% NEQ 0 echo Please restart this batch file with administrator rights.
if %EL% NEQ 0 goto END

if /I %1~ == ~ echo No deployment target folder to remove as the first argument.
if /I %1~ == ~ goto HELP
if /I not %2~ == ~ echo Too many arguments
if /I not %2~ == ~ goto HELP

if not exist %1\nul echo The deployment target  does not exist.
if not exist %1\nul goto HELP
if not exist %1\winexpext.app.exe echo The deployment target folder has no application file: winexpext.app.exe
if not exist %1\winexpext.app.exe echo Please validate the folder path as the argument.
if not exist %1 goto HELP

echo Removing registry settings for Windows Explorer context menu ...
winexpext.app.exe --RemoveExplorerExtensions
SET EL=%ERRORLEVEL%
if %EL% NEQ 0 echo Error removing registry settings (exit code: %EL%)
if %EL% NEQ 0 goto END

echo Removing the deployment folder...
rd %1\ /s /q
SET EL=%ERRORLEVEL%
if %EL% NEQ 0 echo Error removing target folder (exit code: %EL%)
if %EL% NEQ 0 goto END

if exist %1 echo The deployment target folder still exists, but the removal steps did complete.
goto END

:HELP
echo.
echo deploy_to_folder {target_folder}
echo e.g.:
echo    deploy_to_folder c:\appfolder.test\winexpext
echo.
echo Creates target folder when it doesn't exist.
echo Forcefully overwrites files.
echo Adds the registry entries for the Windows Explorer context menu.

echo.

:END

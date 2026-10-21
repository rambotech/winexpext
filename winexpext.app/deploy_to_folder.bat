@echo off

net session 2>>&1 >nul
SET EL=%ERRORLEVEL%
if %EL% NEQ 0 echo This batch file attempts to apply Windows Registry changes for the 
if %EL% NEQ 0 echo application to appear in the Windows Explorer context-menu.
if %EL% NEQ 0 echo This requires the batch file to be run as System Administrator.
if %EL% NEQ 0 echo. 
if %EL% NEQ 0 echo Please restart this batch file with administrator rights.
if %EL% NEQ 0 goto END

if /I %1~ == ~ echo No deployment target folder for deployment as the first argument.
if /I %1~ == ~ goto HELP
if /I not %2~ == ~ echo Too many arguments
if /I not %2~ == ~ goto HELP

if exist %1 goto NOMAKE
echo Creating target folder...
md %1
SET EL=%ERRORLEVEL%
if %EL% NEQ 0 echo Error creating target folder (exit code: %EL%)
if %EL% NEQ 0 goto END

:NOMAKE
echo Switching to target folder...
pushd %1
SET EL=%ERRORLEVEL%
if %EL% NEQ 0 echo Error switching to target folder (exit code: %EL%)
if %EL% NEQ 0 goto END

echo Copy application files ...
copy /y %~d0%~p0\*.* .
SET EL=%ERRORLEVEL%
if %EL% NEQ 0 echo Error copying application files to target folder (exit code: %EL%)
if %EL% NEQ 0 popd
if %EL% NEQ 0 goto END
del remove_from_folder.bat

echo Apply registry settings for Windows Explorer context menu ...
winexpext.app.exe --ApplyExplorerExtensions
SET EL=%ERRORLEVEL%
if %EL% NEQ 0 echo Error applying registry settings (exit code: %EL%)
if %EL% NEQ 0 popd
if %EL% NEQ 0 goto END

REM Post-success, self-destruct (secure) sequence...
popd
(
    echo @echo off
    echo echo Remove deploy_to_folder.bat
    echo del %1\deploy_to_folder.bat
) > %TEMP%\del_deploy_to_folder.bat

del %1\remove_from_folder.bat
%TEMP%\del_deploy_to_folder.bat

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

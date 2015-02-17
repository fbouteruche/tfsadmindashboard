@echo off
:menu
echo RDP:// HyperLink - James Clements - james@jjclements.co.uk
echo ----------------------------------------------------------
echo.
echo.
echo 1. Install RDP Association
echo 2. Uninstall RDP Association
echo 3. Quit
echo.
set choice=
set /p choice=[1,2,3]? 
echo.
if not '%choice%'=='' set choice=%choice:~0,1%
if '%choice%'=='1' goto install
if '%choice%'=='2' goto uninstall
if '%choice%'=='3' goto quit
echo.
echo.
echo "%choice%" is not a valid option - please try again
echo.
pause
cls
goto MENU

:quit
cls
exit

:uninstall
if not exist "C:\Windows\hyperlink-rdp.js" (
cls
echo RDP:// HyperLink - James Clements - james@jjclements.co.uk
echo ----------------------------------------------------------
echo.
echo.
echo RDP:// HyperLink not installed - nothing to remove
echo.
pause
exit)

del "C:\Windows\hyperlink-rdp.js" /f
reg delete "HKCR\rdp" /f

cls
echo RDP:// HyperLink - James Clements - james@jjclements.co.uk
echo ----------------------------------------------------------
echo.
echo.
echo RDP:// HyperLink uninstalled successfully
echo.
pause
exit

:install
if exist "C:\Windows\hyperlink-rdp.js" (
cls
echo RDP:// HyperLink - James Clements - james@jjclements.co.uk
echo ----------------------------------------------------------
echo.
echo.
echo RDP:// HyperLink already installed - nothing to install
echo.
pause
exit)

echo var server=(WScript.Arguments(0))>>C:\Windows\hyperlink-rdp.js
echo var prefix='rdp://'>>C:\Windows\hyperlink-rdp.js
echo var app='C:\\WINDOWS\\system32\\mstsc.exe'>>C:\Windows\hyperlink-rdp.js
echo server=server.replace(prefix, '')>>C:\Windows\hyperlink-rdp.js
echo server=server.replace('/', '')>>C:\Windows\hyperlink-rdp.js
echo var shell = new ActiveXObject("WScript.Shell")>>C:\Windows\hyperlink-rdp.js
echo shell.Exec(app + " /v:" + server)>>C:\Windows\hyperlink-rdp.js

reg add "HKCR\rdp" /f /v "" /t REG_SZ /d "URL:Remote Desktop Connection"
reg add "HKCR\rdp" /f /v "URL Protocol" /t REG_SZ /d ""
reg add "HKCR\rdp\DefaultIcon" /f /v "" /t REG_SZ /d "C:\WINDOWS\System32\mstsc.exe"
reg add "HKCR\rdp\shell\open\command" /f /v "" /t REG_SZ /d "wscript.exe C:\WINDOWS\hyperlink-rdp.js %%1"

cls
echo RDP:// HyperLink - James Clements - james@jjclements.co.uk
echo ----------------------------------------------------------
echo.
echo.
echo RDP:// HyperLink installed successfully
echo.
pause
exit
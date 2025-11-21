@echo off
setlocal

::--------------------------------------------------------------------------------
:: Build
::--------------------------------------------------------------------------------

echo Starting to build the frontend for ASP.NET Core samples ...
echo.

echo Installing Node.js packages ...
echo.
call npm install
if errorlevel 1 goto error
echo.

echo Installing Bower packages ...
echo.
call bower install
if errorlevel 1 goto error
echo.

echo Building client-side assets ...
echo.
call gulp
if errorlevel 1 goto error
echo.

::--------------------------------------------------------------------------------
:: Exit
::--------------------------------------------------------------------------------

echo Succeeded!
goto exit

:error
echo *** Error: The previous step failed!

:exit
cd ../../
endlocal
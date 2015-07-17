@echo off
cd %~dp0

SETLOCAL
SET CACHED_NUGET=%LocalAppData%\NuGet\NuGet.exe

IF EXIST %CACHED_NUGET% goto copynuget
echo Downloading latest version of NuGet.exe...
IF NOT EXIST %LocalAppData%\NuGet md %LocalAppData%\NuGet
@powershell -NoProfile -ExecutionPolicy unrestricted -Command "$ProgressPreference = 'SilentlyContinue'; Invoke-WebRequest 'https://www.nuget.org/nuget.exe' -OutFile '%CACHED_NUGET%'"

:copynuget
IF EXIST .nuget\nuget.exe goto restore
md .nuget
copy %CACHED_NUGET% .nuget\nuget.exe > nul

:restore
IF EXIST packages\KoreBuild goto run
IF DEFINED BUILDCMD_RELEASE (
	.nuget\NuGet.exe install KoreBuild -version 0.2.1-%BUILDCMD_RELEASE% -ExcludeVersion -o packages -nocache -pre
) ELSE (
	.nuget\NuGet.exe install KoreBuild -ExcludeVersion -o packages -nocache -pre
)
.nuget\NuGet.exe install Sake -version 0.2.0 -o packages -ExcludeVersion

:run
CALL packages\KoreBuild\build\dnvm use default -runtime CLR -arch x86
packages\Sake\tools\Sake.exe -I packages\KoreBuild\build -f makefile.shade %*

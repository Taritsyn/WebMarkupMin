@echo off
cd %~dp0

setlocal
set local_nuget_package_manager=.nuget\NuGet.exe
set local_nuget_config_file=.nuget\BuildNuGet.Config
set package_dir=packages

:restore
if exist %package_dir%\KoreBuild goto run
%local_nuget_package_manager% install KoreBuild -Version 0.2.1-beta6 -O %package_dir% -ConfigFile %local_nuget_config_file% -ExcludeVersion -NoCache -Pre
%local_nuget_package_manager% install Sake -Version 0.2.0 -O %package_dir% -ExcludeVersion

:run
call %package_dir%\KoreBuild\build\dnvm use default -runtime CLR -arch x86
%package_dir%\Sake\tools\Sake.exe -I %package_dir%\KoreBuild\build -f makefile.shade %*
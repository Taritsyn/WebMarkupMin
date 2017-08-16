set project_name=WebMarkupMin.AspNetCore2
set dotnet_project_source_dir=..\..\src\%project_name%
set dotnet_project_bin_dir=%dotnet_project_source_dir%\bin\Release
set nuget_package_manager=..\..\.nuget\nuget.exe

call ../setup.cmd

rmdir lib /Q/S

%dotnet_cli% restore "%dotnet_project_source_dir%"

%dotnet_cli% build "%dotnet_project_source_dir%" --framework netstandard2.0 --configuration Release --no-dependencies --no-incremental
xcopy "%dotnet_project_bin_dir%\netstandard2.0\%project_name%.dll" lib\netstandard2.0\ /E
xcopy "%dotnet_project_bin_dir%\netstandard2.0\%project_name%.xml" lib\netstandard2.0\ /E

%nuget_package_manager% pack "..\%project_name%\%project_name%.nuspec"
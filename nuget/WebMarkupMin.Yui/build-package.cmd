set project_name=WebMarkupMin.Yui
set project_source_dir=..\..\src\%project_name%
set project_bin_dir=%project_source_dir%\bin\Release
set nuget_package_manager=..\..\.nuget\nuget.exe

call ../setup.cmd

rmdir lib /Q/S

%dotnet_cli% restore "%project_source_dir%"

%dotnet_cli% build "%project_source_dir%" --framework net40-client --configuration Release --no-dependencies --no-incremental
xcopy "%project_bin_dir%\net40-client\%project_name%.dll" lib\net40-client\ /E
xcopy "%project_bin_dir%\net40-client\%project_name%.xml" lib\net40-client\ /E

%dotnet_cli% build "%project_source_dir%" --framework net45 --configuration Release --no-dependencies --no-incremental
xcopy "%project_bin_dir%\net45\%project_name%.dll" lib\net45\ /E
xcopy "%project_bin_dir%\net45\%project_name%.xml" lib\net45\ /E

%nuget_package_manager% pack "..\%project_name%\%project_name%.nuspec"
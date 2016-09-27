set project_name=WebMarkupMin.Core
set project_source_dir=..\..\src\%project_name%
set project_bin_dir=%project_source_dir%\bin\Release
set nuget_package_manager=..\..\.nuget\nuget.exe

call ../setup.cmd

rmdir lib /Q/S

%net40_msbuild% "%project_source_dir%\%project_name%.Net40.csproj" /p:Configuration=Release
xcopy "%project_bin_dir%\%project_name%.dll" lib\net40-client\

%dotnet_cli% build "%project_source_dir%" --framework net451 --configuration Release --no-dependencies --no-incremental
xcopy "%project_bin_dir%\net451\%project_name%.dll" lib\net451\ /E
xcopy "%project_bin_dir%\net451\%project_name%.xml" lib\net451\ /E

%dotnet_cli% build "%project_source_dir%" --framework netstandard1.3 --configuration Release --no-dependencies --no-incremental
xcopy "%project_bin_dir%\netstandard1.3\%project_name%.dll" lib\netstandard1.3\ /E
xcopy "%project_bin_dir%\netstandard1.3\%project_name%.xml" lib\netstandard1.3\ /E

%nuget_package_manager% pack "..\%project_name%\%project_name%.nuspec"
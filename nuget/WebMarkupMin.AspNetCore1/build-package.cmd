set project_name=WebMarkupMin.AspNetCore1
set project_source_dir=..\..\src\%project_name%
set project_bin_dir=%project_source_dir%\bin\Release
set nuget_package_manager=..\..\.nuget\nuget.exe

call ../setup.cmd

rmdir lib /Q/S

%dotnet_cli% build "%project_source_dir%" --framework net452 --configuration Release --no-dependencies --no-incremental
xcopy "%project_bin_dir%\net452\%project_name%.dll" lib\net452\ /E
xcopy "%project_bin_dir%\net452\%project_name%.xml" lib\net452\ /E

%dotnet_cli% build "%project_source_dir%" --framework netstandard1.3 --configuration Release --no-dependencies --no-incremental
xcopy "%project_bin_dir%\netstandard1.3\%project_name%.dll" lib\netstandard1.3\ /E
xcopy "%project_bin_dir%\netstandard1.3\%project_name%.xml" lib\netstandard1.3\ /E

%nuget_package_manager% pack "..\%project_name%\%project_name%.nuspec"
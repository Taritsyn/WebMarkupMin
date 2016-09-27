set project_name=WebMarkupMin.AspNet4.HttpModules
set net4_project_source_dir=..\..\src\%project_name%
set net4_project_bin_dir=%net4_project_source_dir%\bin\Release
set nuget_package_manager=..\..\.nuget\nuget.exe

call ../setup.cmd

rmdir lib /Q/S

%net40_msbuild% "%net4_project_source_dir%\%project_name%.Net40.csproj" /p:Configuration=Release
xcopy "%net4_project_bin_dir%\%project_name%.dll" lib\net40\

%nuget_package_manager% pack "..\%project_name%\%project_name%.nuspec"
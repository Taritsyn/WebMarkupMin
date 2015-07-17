set project_name=WebMarkupMin.AspNet4.Mvc
set project_source_dir=..\..\src\%project_name%
set nuget_package_manager=..\..\.nuget\nuget.exe

call ../setup.cmd

rmdir lib /Q/S

%net40_msbuild% %project_source_dir%\%project_name%.Net40.csproj /p:Configuration=Release
xcopy %project_source_dir%\bin\Release\%project_name%.dll lib\net40\

%nuget_package_manager% pack ..\%project_name%\%project_name%.nuspec
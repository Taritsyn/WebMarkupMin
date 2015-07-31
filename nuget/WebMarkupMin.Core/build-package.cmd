set project_name=WebMarkupMin.Core
set project_source_dir=..\..\src\%project_name%
set project_artifacts_dir=..\..\artifacts\bin\%project_name%
set nuget_package_manager=..\..\.nuget\nuget.exe

call ../setup.cmd

rmdir lib /Q/S

%net40_msbuild% %project_source_dir%\%project_name%.Net40.csproj /p:Configuration=Release
xcopy %project_source_dir%\bin\Release\%project_name%.dll lib\net40-client\

%dnx_runtime% --appbase %project_source_dir% %dnx_package_manager% build %project_source_dir% --framework dnx451 --configuration Release --out %project_artifacts_dir%
xcopy %project_artifacts_dir%\Release\dnx451\%project_name%.dll lib\dnx451\ /E
xcopy %project_artifacts_dir%\Release\dnx451\%project_name%.xml lib\dnx451\ /E

%dnx_runtime% --appbase %project_source_dir% %dnx_package_manager% build %project_source_dir% --framework dotnet --configuration Release --out %project_artifacts_dir%
xcopy %project_artifacts_dir%\Release\dotnet\%project_name%.dll lib\dotnet\ /E
xcopy %project_artifacts_dir%\Release\dotnet\%project_name%.xml lib\dotnet\ /E

%nuget_package_manager% pack ..\%project_name%\%project_name%.nuspec
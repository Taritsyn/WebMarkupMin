set project_name=WebMarkupMin.AspNet5
set project_source_dir=..\..\src\%project_name%
set project_artifacts_dir=..\..\artifacts\bin\%project_name%
set nuget_package_manager=..\..\.nuget\nuget.exe

call ../setup.cmd

rmdir lib /Q/S

%dnx_runtime% --appbase %project_source_dir% %dnx_package_manager% build %project_source_dir% --framework dnx451 --configuration Release --out %project_artifacts_dir%
xcopy %project_artifacts_dir%\Release\dnx451\%project_name%.dll lib\dnx451\ /E
xcopy %project_artifacts_dir%\Release\dnx451\%project_name%.xml lib\dnx451\ /E

%dnx_runtime% --appbase %project_source_dir% %dnx_package_manager% build %project_source_dir% --framework dnxcore50 --configuration Release --out %project_artifacts_dir%
xcopy %project_artifacts_dir%\Release\dnxcore50\%project_name%.dll lib\dnxcore50\ /E
xcopy %project_artifacts_dir%\Release\dnxcore50\%project_name%.xml lib\dnxcore50\ /E

%nuget_package_manager% pack ..\%project_name%\%project_name%.nuspec
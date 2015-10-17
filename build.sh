#!/bin/bash

local_nuget_package_manager=.nuget\NuGet.exe
local_nuget_config_file=.nuget\BuildNuGet.Config
package_dir=packages

if test ! -d $package_dir/Sake; then
  mono $local_nuget_package_manager install KoreBuild -Version 0.2.1-beta8 -O $package_dir -ConfigFile $local_nuget_config_file -ExcludeVersion -NoCache -Pre
  mono $local_nuget_package_manager install Sake -Version 0.2.2 -O $package_dir -Source https://www.nuget.org/api/v2/ -ExcludeVersion
fi

if ! type dnvm > /dev/null 2>&1; then
    source packages/KoreBuild/build/dnvm.sh
fi

mono $package_dir/Sake/tools/Sake.exe -I $package_dir/KoreBuild/build -f makefile.shade "$@"
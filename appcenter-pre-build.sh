#!/usr/bin/env bash

echo "Pre-build script executing..."

cp $APPCENTER_SOURCE_DIRECTORY/DotNetRu.AppUtils/Config/config.$Environment.json $APPCENTER_SOURCE_DIRECTORY/DotNetRu.AppUtils/Config/config.json

msBuildMonoPath='/Library/Frameworks/Mono.framework/Versions/6.12.0/lib/mono/msbuild/15.0/bin/MSBuild.dll'
msBuildVSforMacPath='/Applications/Visual\\ Studio.app/Contents/MonoBundle/MSBuild/Current/bin/MSBuild.dll'

# Use msbuild from Visual Studio
sudo sed -i '' "s#$msBuildMonoPath#$msBuildVSforMacPath#" /Library/Frameworks/Mono.framework/Commands/msbuild
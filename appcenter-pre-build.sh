#!/usr/bin/env bash

echo "Pre-build script executing..."

cp $APPCENTER_SOURCE_DIRECTORY/DotNetRu.AppUtils/Config/config.$Environment.json $APPCENTER_SOURCE_DIRECTORY/DotNetRu.AppUtils/Config/config.json

# Use msbuild from Visual Studio
sudo sed -i '' 's#/Library/Frameworks/Mono.framework/Versions/6.12.0/lib/mono/msbuild/15.0/bin/MSBuild.dll#/Applications/Visual\ Studio.app/Contents/MonoBundle/MSBuild/Current/bin/MSBuild.dll#' /Library/Frameworks/Mono.framework/Commands/msbuild
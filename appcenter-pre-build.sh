#!/usr/bin/env bash

echo "Pre-build script executing..."

cp $APPCENTER_SOURCE_DIRECTORY/DotNetRu.AppUtils/Config/config.$Environment.json $APPCENTER_SOURCE_DIRECTORY/DotNetRu.AppUtils/Config/config.json

# Add support for C# 10
MonoFrameworkPackage=MonoFramework-MDK-6.12.0.182.macos10.xamarin.universal.pkg
wget https://download.mono-project.com/archive/6.12.0/macos-10-universal/$MonoFrameworkPackage
sudo chmod +x $MonoFrameworkPackage
sudo installer -pkg $MonoFrameworkPackage -target /
#!/usr/bin/env bash

if [ "$APPCENTER_BRANCH" == "master" ];
then
  sed -i '' "s/6f9a7703-8ca4-477e-9558-7e095f7d20aa/33fe8e32-1b4f-41a1-a835-88f345b31f98/" $APPCENTER_SOURCE_DIRECTORY/DotNetRu.Clients.UI/App.xaml.cs
fi
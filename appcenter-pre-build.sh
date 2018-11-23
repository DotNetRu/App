#!/usr/bin/env bash

if [ "$APPCENTER_BRANCH" == "master" ];
then
  sed -i '' "s/1e7f311f-1055-4ec9-8b00-0302015ab8ae/$AppCenterKey/" $APPCENTER_SOURCE_DIRECTORY/DotNetRu.Clients.UI/App.xaml.cs
fi
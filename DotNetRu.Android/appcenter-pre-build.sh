#!/usr/bin/env bash

if [ "$APPCENTER_BRANCH" == "master" ];
then
  sed -i '' "s/6f9a7703-8ca4-477e-9558-7e095f7d20aa/$AppCenterKey/" $APPCENTER_SOURCE_DIRECTORY/DotNetRu.Clients.UI/App.xaml.cs
fi
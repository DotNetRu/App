#!/usr/bin/env bash

if [ "$APPCENTER_BRANCH" != "master" ];
then
  sed -i 's/6f9a7703-8ca4-477e-9558-7e095f7d20aa/79c348e1-2cfe-4ba6-b665-95d5fd6d5c80/g' $APPCENTER_SOURCE_DIRECTORY/DotNetRu.Clients.UI/App.xaml.cs
fi
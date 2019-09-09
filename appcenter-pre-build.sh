#!/usr/bin/env bash

sed -i '' "s/APP_CENTER_ANDROID_KEY/$AppCenterAndroidKey/" $APPCENTER_SOURCE_DIRECTORY/DotNetRu.Utils/Config/config.json
sed -i '' "s/APP_CENTER_IOS_KEY/$AppCenteriOSKey/" $APPCENTER_SOURCE_DIRECTORY/DotNetRu.Utils/Config/config.json
sed -i '' "s%TWEET_FUNCTION_URL%$TweetFunctionURLKey%" $APPCENTER_SOURCE_DIRECTORY/DotNetRu.Utils/Config/config.json
sed -i '' "s%REALM_DATABASE%$RealmDatabase%" $APPCENTER_SOURCE_DIRECTORY/DotNetRu.Utils/Config/config.json
sed -i '' "s%REALM_SERVER_URL%$RealmServerUrl%" $APPCENTER_SOURCE_DIRECTORY/DotNetRu.Utils/Config/config.json
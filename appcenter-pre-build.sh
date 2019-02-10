#!/usr/bin/env bash

sed -i '' "s/APP_CENTER_ANDROID_KEY/$AppCenterAndroidKey/" $APPCENTER_SOURCE_DIRECTORY/DotNetRu.Utils/Config/config.json
sed -i '' "s/APP_CENTER_IOS_KEY/$AppCenteriOSKey/" $APPCENTER_SOURCE_DIRECTORY/DotNetRu.Utils/Config/config.json
sed -i '' "s/PUSH_NOTIFICATIONS_CHANNEL/$PushNotificationsChannelKey/" $APPCENTER_SOURCE_DIRECTORY/DotNetRu.Utils/Config/config.json
sed -i '' "s/UPDATE_FUNCTION_URL/$UpdateFunctionURLKey/" $APPCENTER_SOURCE_DIRECTORY/DotNetRu.Utils/Config/config.json
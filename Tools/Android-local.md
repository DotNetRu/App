# Run DotNetRu on the Pixel 5 emulator

This path uses the Xamarin.Android installation already present on the Apple Silicon Mac: Xamarin.Android 13.2, Mono with Rosetta, Visual Studio for Mac's bundled MSBuild, the ARM64 .NET SDK 7.0.203, and Microsoft JDK 11. The Android SDK is under `~/Library/Developer/Xamarin/android-sdk-macosx`. It contains API 30 for building and an ARM64 API 33 image for running.

Run from the repository root. Use the .NET SDK to restore the Android project and build its shared C# projects. Use Mono MSBuild for the APK because some of the legacy Xamarin packaging tasks require .NET Framework assemblies.

```sh
export DOTNET_ROOT=/usr/local/share/dotnet
android_sdk="$HOME/Library/Developer/Xamarin/android-sdk-macosx"
android_project=DotNetRu.Android/DotNetRu.Android.csproj
xamarin_targets=/Library/Frameworks/Xamarin.Android.framework/Versions/Current/lib/xamarin.android/xbuild/Xamarin/Android/Xamarin.Android.CSharp.targets
mono_msbuild='/Applications/Visual Studio.app/Contents/MonoBundle/MSBuild/Current/bin/MSBuild.dll'

"$DOTNET_ROOT/dotnet" restore "$android_project" -p:XamarinAndroidTargets="$xamarin_targets"
"$DOTNET_ROOT/dotnet" build DotNetRu.Clients.UI/DotNetRu.Clients.UI.csproj --no-restore

MSBuildSDKsPath="$DOTNET_ROOT/sdk/7.0.203/Sdks" MSBuildEnableWorkloadResolver=false \
  mono "$mono_msbuild" "$android_project" -t:SignAndroidPackage \
  -p:XamarinAndroidTargets="$xamarin_targets" \
  -p:BuildProjectReferences=false \
  -p:AndroidSdkDirectory="$android_sdk" \
  -p:JavaSdkDirectory=/Library/Java/JavaVirtualMachines/microsoft-11.jdk/Contents/Home \
  -p:AndroidUseSharedRuntime=false -p:AndroidSupportedAbis=arm64-v8a
```

If the emulator isn't already running, boot it in a separate terminal. The read-only session leaves the existing AVD unchanged when it shuts down.

```sh
android_sdk="$HOME/Library/Developer/Xamarin/android-sdk-macosx"
ANDROID_HOME="$android_sdk" "$android_sdk/emulator/emulator" \
  -avd pixel_5_-_api_33 -read-only -no-snapshot -no-boot-anim
```

List the devices and select DotNetRu's emulator serial. Wait for `sys.boot_completed` to return `1` before installing and launching the debug APK.

```sh
adb="$HOME/Library/Developer/Xamarin/android-sdk-macosx/platform-tools/adb"
"$adb" devices -l
android_device=emulator-5554 # Replace with the Pixel 5 serial from the list.
"$adb" -s "$android_device" shell getprop sys.boot_completed
"$adb" -s "$android_device" install -r DotNetRu.Android/bin/Debug/com.dotnetru.droid-Signed.apk
activity=$("$adb" -s "$android_device" shell cmd package resolve-activity --brief \
  -a android.intent.action.MAIN -c android.intent.category.LAUNCHER com.dotnetru.droid | tail -n 1 | tr -d '\r')
"$adb" -s "$android_device" shell am start -W -n "$activity"
```

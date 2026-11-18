# Build DotNetRu for iOS Simulator

This build uses the Xamarin.iOS 16.2 installation on this Apple Silicon Mac, the ARM64 .NET SDK 7.0.203, Visual Studio for Mac’s bundled Mono MSBuild, and Xcode 27.0. The Xamarin simulator libraries in this installation target x86_64, so running the bundle requires Rosetta and a Universal iOS Simulator runtime.

Run from the repository root:

```sh
export DOTNET_ROOT=/usr/local/share/dotnet
ios_project=DotNetRu.iOS/DotNetRu.iOS.csproj
xamarin_targets=/Library/Frameworks/Xamarin.iOS.framework/Versions/Current/lib/msbuild/iOS/Xamarin.iOS.CSharp.targets
mono_msbuild='/Applications/Visual Studio.app/Contents/MonoBundle/MSBuild/Current/bin/MSBuild.dll'

"$DOTNET_ROOT/dotnet" restore "$ios_project" \
  -p:XamarinIosTargets="$xamarin_targets" -p:Platform=iPhoneSimulator \
  -p:RuntimeIdentifiers=iossimulator-x64

"$DOTNET_ROOT/dotnet" build DotNetRu.Clients.UI/DotNetRu.Clients.UI.csproj \
  --no-restore -p:Platform=iPhoneSimulator

MSBuildSDKsPath="$DOTNET_ROOT/sdk/7.0.203/Sdks" MSBuildEnableWorkloadResolver=false \
  mono "$mono_msbuild" "$ios_project" \
  -p:XamarinIosTargets="$xamarin_targets" -p:Platform=iPhoneSimulator \
  -p:BuildProjectReferences=false -p:NuGetRuntimeIdentifier=iossimulator-x64 \
  -p:EnableCodeSigning=false -p:MtouchLink=SdkOnly
```

Specifying the runtime identifier on both restore and packaging lets the legacy NuGet targets resolve the iOS implementations of packages. `MtouchLink=SdkOnly` trims unused SDK bindings, including the reference to `NewsstandKit` that fails to link with Xcode 27. The bundle is written to `DotNetRu.iOS/bin/iPhoneSimulator/Debug/DotNetRu.iOS.app`.

Check `xcrun simctl runtime list` first. If the iOS 26.5 Universal runtime is missing, install it with the command below. The iOS 27.0 runtime offered by this Xcode is ARM64 only. The Universal download is about 10.6 GB.

```sh
xcodebuild -downloadPlatform iOS -buildVersion 26.5 -architectureVariant universal
xcrun simctl list runtimes
```

Use `xcrun simctl list devices available` to find an existing DotNetRu iPhone 17 on iOS 26.5. If it exists, set `simulator` to its UDID and skip `simctl create`. Boot the simulator normally and request Intel translation when launching DotNetRu. Skip `simctl boot` if the selected device is already booted.

```sh
simulator=$(xcrun simctl create 'DotNetRu iPhone 17' \
  com.apple.CoreSimulator.SimDeviceType.iPhone-17 \
  com.apple.CoreSimulator.SimRuntime.iOS-26-5)
xcrun simctl boot "$simulator"
xcrun simctl bootstatus "$simulator" -b
open /Applications/Xcode.app/Contents/Applications/DeviceHub.app
xcrun simctl install "$simulator" DotNetRu.iOS/bin/iPhoneSimulator/Debug/DotNetRu.iOS.app
xcrun simctl launch --arch=x86_64 "$simulator" com.pfedotovsky.dotnetru
```

On this Mac, the first boot in Intel mode stalled before it could render a screen. A normal boot followed by `launch --arch=x86_64` opened DotNetRu. Confirm that News and the tab bar render in Device Hub. On a fresh install, iOS may show the notification prompt over the app.

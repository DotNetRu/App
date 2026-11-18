# DotNetRu agent guide

This checkout contains the Xamarin.Forms app for the DotNetRu .NET user groups. Use [the project guide](docs/PROJECT.md) to find entry points, data sources, and prior migration work.

## Working in the repository

- Start with `DotNetRuApp.sln` for mobile work and `DotNetRuBackend.sln` for the ASP.NET Core service. Shared screens and view models live in `DotNetRu.Clients.UI`. Platform startup and renderers live in `DotNetRu.Android` and `DotNetRu.iOS`.
- Follow [the contribution guide](CONTRIBUTING.md) and [`.editorconfig`](.editorconfig). The repository targets `develop` for pull requests.
- Preserve embedded resource names when editing localization, XAML, or the offline Realm database. Check the consuming code and the `LogicalName` entries in the relevant project file.
- Use the current Xamarin projects as the local build baseline. The MAUI branches are migration references. Treat the older checkout at `~/Projects/App` as read-only when consulting local branches.

## Build and verify

- On Apple Silicon, use the [Android runbook](Tools/Android-local.md) or the [iOS runbook](Tools/iOS-local.md). These recipes target one mobile project at a time. They restore and build the shared code with .NET SDK 7.0.203, then package with Visual Studio for Mac's Mono MSBuild.
- Use the runbooks for local builds. The historical `appcenter-pre-build.sh` replaces the embedded app configuration and edits the installed Mono `msbuild` launcher.
- Check for existing SDKs and running virtual devices before installing tools or creating devices. Target DotNetRu's device explicitly when more than one simulator or emulator is running.
- For a launch smoke test, verify that the app renders its own UI after startup. The Meetups tab can check the bundled archive without a working live feed. Report the platform and device, the screen verified, and any behavior that still needs testing.
- `DotNetRu.RealmTests` is a .NET Core 3.1 console program that exercises offline initialization and cloud sync. Use it only when its runtime and service dependencies are available.

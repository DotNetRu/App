# DotNetRu project guide

DotNetRu is a Xamarin.Forms app for Russian-speaking .NET user groups. The shared shell exposes News, Speakers, Meetups, and About. The app ships with an offline archive of meetups and speakers. The News tab uses live social feeds.

## Code map

| Area | Where to start |
| --- | --- |
| Mobile solution | [`DotNetRuApp.sln`](../DotNetRuApp.sln) |
| Shared UI and navigation | [`App.xaml.cs`](../DotNetRu.Clients.UI/App.xaml.cs) initializes app state. [`AppShell.xaml`](../DotNetRu.Clients.UI/AppShell.xaml) defines the tabs. Pages and view models are grouped by feature in `DotNetRu.Clients.UI`. |
| Android host | [`MainActivity.cs`](../DotNetRu.Android/MainActivity.cs) initializes Xamarin.Forms and loads the shared app. |
| iOS host | [`Main.cs`](../DotNetRu.iOS/Main.cs) enters UIKit. [`AppDelegate.cs`](../DotNetRu.iOS/AppDelegate.cs) initializes Xamarin.Forms and loads the shared app. |
| Runtime configuration | [`AppConfig.cs`](../DotNetRu.AppUtils/Config/AppConfig.cs) reads `config.json` embedded in `DotNetRu.AppUtils`. The project also contains `config.DEV.json` and `config.PROD.json` for App Center builds. |
| Models and offline data | [`DotNetRu.Models`](../DotNetRu.Models) contains Realm entities. [`RealmService.cs`](../DotNetRu.DataStore.Audit/Services/RealmService.cs) initializes the mobile store from the embedded `DotNetRuOffline.realm`. [`RealmUpdateLibrary`](../RealmUpdateLibrary) contains the audit import and update logic. |
| Backend | [`DotNetRuBackend.sln`](../DotNetRuBackend.sln) contains the .NET 7 ASP.NET Core [`DotNetRu.AppService`](../DotNetRu.AppService), including feed, Realm, and push notification controllers. [`RealmController.cs`](../DotNetRu.AppService/Controllers/RealmController.cs) includes the offline Realm generation endpoint. |

The bundled Realm records [DotNetRu/Audit revision `65e8ba4`](https://github.com/DotNetRu/Audit/commit/65e8ba444d2526f476bb6e10ed21b4ae1ef42067) in its `AuditVersion` row. It contains 204 meetups, including Встреча SpbDotNet №119 on May 27, 2026.

English and Russian strings live in [`DotNetRu.Clients.UI/Localization`](../DotNetRu.Clients.UI/Localization). The project file sets explicit resource names so loading works with the .NET SDK on macOS.

## Verified local development baseline

The recipes assume Xamarin and the older SDK are already installed on this Apple Silicon Mac. The `dotnet` binary on `PATH` may resolve to a newer Homebrew SDK. Use the explicit executable in the runbooks.

| Platform | Verified | Runbook |
| --- | --- | --- |
| Android | Built a signed debug APK, cold launched on a Pixel 5 ARM64 emulator running API 33, and opened Meetups with archived events. News rendered with an empty feed. | [Android](../Tools/Android-local.md) |
| iOS | Built an x86_64 simulator bundle, launched on an iPhone 17 running the iOS 26.5 Universal runtime, and saw News, the tab bar, and the notification prompt. The simulator boots as ARM64 and translates the Xamarin app through Rosetta. | [iOS](../Tools/iOS-local.md) |

These checks cover local packaging and startup. The backend, live feeds, push delivery, and physical devices still need separate verification. [`DotNetRu.RealmTests`](../DotNetRu.RealmTests/Program.cs) is a .NET Core 3.1 console utility that exercises offline Realm initialization.

## Migration references

- `origin/feature/upgrade-to-maui` at `08092ea` contains a .NET 7 MAUI scaffold. Its `DotNetRu.App/MainPage.xaml.cs` still implements the template counter. Use the branch to see where migration work started.
- The older checkout at `~/Projects/App` has `feature/move-to-maui` at `95c1728`, a partial .NET 6 MAUI port, and `feature/add-support-for-net7` at `f231ca4`, which changes the App Center build script. Both are useful historical references. Inspect them with `git -C ~/Projects/App show` or `git -C ~/Projects/App log`.

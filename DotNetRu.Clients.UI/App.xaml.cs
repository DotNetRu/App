using System.Globalization;

using DotNetRu.Clients.Portable.Services;
using DotNetRu.Clients.Portable.ViewModel;
using DotNetRu.Clients.UI.Localization;
using DotNetRu.DataStore.Audit.Services;

using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Push;
using Realms.Sync;
using DotNetRu.AppUtils.Logging;
using DotNetRu.AppUtils.Config;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace DotNetRu.Clients.UI
{
    public partial class App
    {
        private static ILogger logger;

        public static bool IsAppCtorCalled { get; set; } = false;

        public static bool IsOnStartCalled { get; set; } = false;

        public App()
        {
            // workaround for https://github.com/realm/realm-dotnet/issues/1967
            if (Xamarin.Forms.Device.RuntimePlatform != Xamarin.Forms.Device.iOS)
            {
                SyncConfigurationBase.Initialize(UserPersistenceMode.NotEncrypted);
            }

            VersionTracking.Track();

            var language = LanguageService.GetCurrentLanguage();
            AppResources.Culture = new CultureInfo(language.GetLanguageCode());
           
            if (!IsAppCtorCalled)
            {
                RealmService.InitializeOfflineDatabase();

                IsAppCtorCalled = true;
            }

            this.InitializeComponent();
            this.MainPage = new AppShell();
        }

        // TODO change to SeriLog
        public static ILogger Logger => logger ?? (logger = DependencyService.Get<ILogger>());

        public void SecondOnResume()
        {
            this.OnResume();
        }

        public new void SendOnAppLinkRequestReceived(Uri uri)
        {
            this.OnAppLinkRequestReceived(uri);
        }

        protected override async void OnStart()
        {
            if (IsOnStartCalled)
            {
                return;
            }

            var config = AppConfig.GetConfig();

            AppCenter.Start(
                $"ios={config.AppCenteriOSKey};android={config.AppCenterAndroidKey};",
                typeof(Analytics),
                typeof(Crashes),
                typeof(Push));

            Analytics.TrackEvent("AppStarted", new Dictionary<string, string>()
                {
                    {nameof(config.AppCenterAndroidKey), config.AppCenterAndroidKey },
                    {nameof(config.AppCenteriOSKey), config.AppCenteriOSKey },
                    {nameof(config.RealmServerUrl), config.RealmServerUrl },
                    {nameof(config.RealmDatabase), config.RealmDatabase },
                    {"TimeZone", TimeZoneInfo.Local.ToSerializedString()}
                });

            await RealmService.InitializeCloudSync(config.RealmServerUrl, config.RealmDatabase);

            IsOnStartCalled = true;
        }

        protected override void OnResume()
        {

        }

        protected override void OnAppLinkRequestReceived(Uri uri)
        {
            base.OnAppLinkRequestReceived(uri);
        }

        protected override void OnSleep()
        {

        }
    }
}

using Xamarin.Forms.Xaml;
using System;
using System.Globalization;

using DotNetRu.Clients.Portable.Interfaces;
using DotNetRu.Clients.Portable.Services;
using DotNetRu.Clients.Portable.ViewModel;
using DotNetRu.Clients.UI.Localization;
using DotNetRu.Clients.UI.Pages;
using DotNetRu.DataStore.Audit.Services;
using DotNetRu.Utils.Helpers;
using DotNetRu.Utils.Interfaces;

using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Push;
using Xamarin.Essentials;
using Xamarin.Forms;
using System.Collections.Generic;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace DotNetRu.Clients.UI
{
    public partial class App
    {
        private static ILogger logger;

        private bool registered;

        public static bool IsAppCtorCalled { get; set; } = false;

        public static bool IsOnStartCalled { get; set; } = false;

        public App()
        {
            VersionTracking.Track();

            var language = LanguageService.GetCurrentLanguage();
            AppResources.Culture = new CultureInfo(language.GetLanguageCode());
           
            if (!IsAppCtorCalled)
            {
                RealmService.InitializeOfflineDatabase();

                IsAppCtorCalled = true;
            }

            this.InitializeComponent();
            this.MainPage = new BottomTabbedPage();
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
            Settings.IsConnected = Connectivity.NetworkAccess == NetworkAccess.Internet;
            Connectivity.ConnectivityChanged += this.ConnectivityChanged;
        }

        protected override void OnAppLinkRequestReceived(Uri uri)
        {
            base.OnAppLinkRequestReceived(uri);
        }

        protected override void OnSleep()
        {
            Connectivity.ConnectivityChanged -= this.ConnectivityChanged;
        }

        protected void ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            var currentlyConnected = e.NetworkAccess == NetworkAccess.Internet;

            // save current state and then set it
            var wasConnected = Settings.IsConnected;
            Settings.IsConnected = currentlyConnected;
            if (wasConnected && !currentlyConnected)
            {
                var toaster = DependencyService.Get<IToast>();
                toaster.SendToast(
                    "Uh Oh, It looks like you have gone offline. Check your internet connection to get the latest data.");
            }
        }
    }
}

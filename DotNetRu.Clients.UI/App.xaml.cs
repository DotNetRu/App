using Xamarin.Forms.Xaml;
using System;
using System.Globalization;

using DotNetRu.Clients.Portable.Interfaces;
using DotNetRu.Clients.Portable.Model;
using DotNetRu.Clients.Portable.Services;
using DotNetRu.Clients.Portable.ViewModel;
using DotNetRu.Clients.UI.Localization;
using DotNetRu.Clients.UI.Pages;
using DotNetRu.DataStore.Audit.Services;
using DotNetRu.Utils.Helpers;
using DotNetRu.Utils.Interfaces;

using FormsToolkit;

using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Push;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace DotNetRu.Clients.UI
{
    public partial class App
    {
        private static ILogger logger;

        private bool registered;

        public App()
        {
            VersionTracking.Track();

            var language = LanguageService.GetCurrentLanguage();
            AppResources.Culture = new CultureInfo(language.GetLanguageCode());

            RealmService.Initialize();

            this.InitializeComponent();

            var config = AppConfig.GetConfig();

            AppCenter.Start(
                $"ios={config.AppCenteriOSKey};android={config.AppCenterAndroidKey};",
                typeof(Analytics),
                typeof(Crashes),
                typeof(Push));

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

        protected override void OnResume()
        {
            if (this.registered)
            {
                return;
            }

            this.registered = true;

            // Handle when your app resumes
            Settings.Current.IsConnected = Connectivity.NetworkAccess == NetworkAccess.Internet;
            Connectivity.ConnectivityChanged += this.ConnectivityChanged;
        }

        protected override void OnAppLinkRequestReceived(Uri uri)
        {
            var data = uri.ToString().ToLowerInvariant();

            // only if deep linking
            if (!data.Contains($"/{AboutThisApp.SessionsSiteSubdirectory.ToLowerInvariant()}/")
                && !data.Contains($"/{AboutThisApp.SpeakersSiteSubdirectory.ToLowerInvariant()}/"))
            {
                return;
            }

            var id = data.Substring(data.LastIndexOf("/", StringComparison.Ordinal) + 1);

            if (!string.IsNullOrWhiteSpace(id))
            {
                AppPage destination = AppPage.Talk;
                if (data.Contains($"/{AboutThisApp.SpeakersSiteSubdirectory.ToLowerInvariant()}/"))
                {
                    destination = AppPage.Speaker;
                }

                MessagingService.Current.SendMessage(
                    "DeepLinkPage",
                    new DeepLinkPage { Page = destination, Id = id.Replace("#", string.Empty) });
            }

            base.OnAppLinkRequestReceived(uri);
        }

        protected override void OnSleep()
        {
            if (!this.registered)
            {
                return;
            }

            this.registered = false;

            // Handle when your app sleeps
            Connectivity.ConnectivityChanged -= this.ConnectivityChanged;
        }

        protected void ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            var currentlyConnected = e.NetworkAccess == NetworkAccess.Internet;

            // save current state and then set it
            var wasConnected = Settings.Current.IsConnected;
            Settings.Current.IsConnected = currentlyConnected;
            if (wasConnected && !currentlyConnected)
            {
                var toaster = DependencyService.Get<IToast>();
                toaster.SendToast(
                    "Uh Oh, It looks like you have gone offline. Check your internet connection to get the latest data.");
            }
        }
    }
}

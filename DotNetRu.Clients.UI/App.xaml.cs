using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace DotNetRu.Clients.UI
{
    using System;
    using System.Globalization;
    using System.Text;
    using System.Threading.Tasks;
    
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
    using Newtonsoft.Json;
    using Xamarin.Essentials;
    using Xamarin.Forms;

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

            // Update Audit on startup. Temporarily disabled due to update issues
            // Task.Run(UpdateService.UpdateAudit);

            var config = AppConfig.GetConfig();

            // This should come before AppCenter.Start() is called
            // Avoid duplicate event registration:
            if (!AppCenter.Configured)
            {
                Push.PushNotificationReceived += (sender, e) =>
                {
                    // Add the notification message and title to the message
                    var summary = $"Push notification received:" +
                                        $"\n\tNotification title: {e.Title}" +
                                        $"\n\tMessage: {e.Message}";

                    // If there is custom data associated with the notification,
                    // print the entries
                    if (e.CustomData != null)
                    {
                        summary += "\n\tCustom data:\n";
                        foreach (var key in e.CustomData.Keys)
                        {
                            summary += $"\t\t{key} : {e.CustomData[key]}\n";
                        }
                    }

                    Logger.Track("PushReceived for iOS app");
                };
            }

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

            // Handle when your app starts
            MessagingService.Current.Subscribe<MessagingServiceAlert>(
                MessageKeys.Message,
                async (m, info) =>
                    {
                        var task = Current?.MainPage?.DisplayAlert(info.Title, info.Message, info.Cancel);

                        if (task == null)
                        {
                            return;
                        }

                        await task;
                        info.OnCompleted?.Invoke();
                    });

            MessagingService.Current.Subscribe<MessagingServiceQuestion>(
                MessageKeys.Question,
                async (m, q) =>
                    {
                        var task = Current?.MainPage?.DisplayAlert(q.Title, q.Question, q.Positive, q.Negative);
                        if (task == null)
                        {
                            return;
                        }

                        var result = await task;
                        q.OnCompleted?.Invoke(result);
                    });

            MessagingService.Current.Subscribe<MessagingServiceChoice>(
                MessageKeys.Choice,
                async (m, q) =>
                    {
                        var task = Current?.MainPage?.DisplayActionSheet(q.Title, q.Cancel, q.Destruction, q.Items);
                        if (task == null)
                        {
                            return;
                        }

                        var result = await task;
                        q.OnCompleted?.Invoke(result);
                    });
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

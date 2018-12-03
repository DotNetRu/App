using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace DotNetRu.Clients.UI
{
    using System;
    using System.Globalization;
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

    using Plugin.Connectivity;
    using Plugin.Connectivity.Abstractions;

    using Xamarin.Forms;

    public partial class App
    {
        private const string IosAppCenterKey = "1e7f311f-1055-4ec9-8b00-0302015ab8ae";

        private const string AndroidAppCenterKey = "6f9a7703-8ca4-477e-9558-7e095f7d20aa";

        private static ILogger logger;

        private bool registered;

        public App()
        {
            var language = LanguageService.GetCurrentLanguage();
            AppResources.Culture = new CultureInfo(language.GetLanguageCode());

            var savedAppVersion = Portable.Helpers.Settings.AppVersion;
            var currentAppVersion = DependencyService.Get<IAppVersionProvider>().AppVersion;

            bool overwrite = savedAppVersion != currentAppVersion;        
            RealmService.Initialize(overwrite);

            Portable.Helpers.Settings.AppVersion = currentAppVersion;

            this.InitializeComponent();

            // Update Audit on startup
            Task.Run(UpdateService.UpdateAudit);

            AppCenter.Start(
                $"ios={IosAppCenterKey};android={AndroidAppCenterKey};",
                typeof(Analytics),
                typeof(Crashes));

            Console.WriteLine("AuditUpdate. AppCenter InstallId: " + AppCenter.GetInstallIdAsync().Result);

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
            Settings.Current.IsConnected = CrossConnectivity.Current.IsConnected;
            CrossConnectivity.Current.ConnectivityChanged += this.ConnectivityChanged;

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
            CrossConnectivity.Current.ConnectivityChanged -= this.ConnectivityChanged;
        }

        protected void ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            // save current state and then set it
            var connected = Settings.Current.IsConnected;
            Settings.Current.IsConnected = e.IsConnected;
            if (connected && !e.IsConnected)
            {
                var toaster = DependencyService.Get<IToast>();
                toaster.SendToast(
                    "Uh Oh, It looks like you have gone offline. Check your internet connection to get the latest data.");
            }
        }
    }
}

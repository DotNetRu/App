using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace DotNetRu.Clients.UI
{
    using System;
    using System.Globalization;
    using System.Threading.Tasks;

    using DotNetRu.Clients.Portable.ApplicationResources;
    using DotNetRu.Clients.Portable.Interfaces;
    using DotNetRu.Clients.Portable.Model;
    using DotNetRu.Clients.Portable.Services;
    using DotNetRu.Clients.Portable.ViewModel;
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

    using XamarinEvolve.Clients.Portable;

    using Device = Xamarin.Forms.Device;

    public partial class App
    {
        private static ILogger logger;

        private bool registered;

        public App()
        {
            var language = LanguageService.GetCurrentLanguage();

            AppResources.Culture = new CultureInfo(language.GetLanguageCode());

            RealmService.Initialize();

            this.InitializeComponent();

#if RELEASE
            AppCenter.Start(
                "ios=1e7f311f-1055-4ec9-8b00-0302015ab8ae;android=6f9a7703-8ca4-477e-9558-7e095f7d20aa;",
                typeof(Analytics),
                typeof(Crashes));
#endif

            this.MainPage = new BottomTabbedPage();
        }

        public static ILogger Logger => logger ?? (logger = DependencyService.Get<ILogger>());

        public void SecondOnResume()
        {
            this.OnResume();
        }

        public new void SendOnAppLinkRequestReceived(Uri uri)
        {
            this.OnAppLinkRequestReceived(uri);
        }

        public async Task Finish()
        {
            if (Device.RuntimePlatform == Device.iOS && Settings.Current.FirstRun)
            {
                var push = DependencyService.Get<IPushNotifications>();
                if (push != null)
                {
                    await push.RegisterForNotifications();
                }
            }
        }

        protected override void OnStart()
        {
            this.OnResume();
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
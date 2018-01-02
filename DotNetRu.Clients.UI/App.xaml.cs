using System;
using System.Globalization;
using System.Threading.Tasks;
using DotNetRu.Clients.Portable.ApplicationResources;
using DotNetRu.Clients.Portable.Interfaces;
using DotNetRu.Clients.Portable.Model;
using DotNetRu.Clients.Portable.ViewModel;
using DotNetRu.Clients.UI.Pages;
using DotNetRu.Clients.UI.Pages.Home;
using DotNetRu.Clients.UI.Pages.iOS;
using DotNetRu.Clients.UI.Pages.Sessions;
using FormsToolkit;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;
using Xamarin.Forms;
using XamarinEvolve.Clients.Portable;
using XamarinEvolve.Utils.Helpers;

namespace DotNetRu.Clients.UI
{
    using Device = Xamarin.Forms.Device;

    public partial class App
    {
        private static ILogger logger;

        private bool registered;

        private bool firstRun = true;

        public App()
        {
            var savedLanguage = XamarinEvolve.Clients.Portable.Helpers.Settings.CurrentLanguage;
            var uiLanguage = DependencyService.Get<ILocalize>().GetCurrentCultureInfo().TwoLetterISOLanguageName == "ru"
                                 ? Language.Russian
                                 : Language.English;

            var language = savedLanguage ?? uiLanguage;

            AppResources.Culture = new CultureInfo(language.GetLanguageCode());

            this.InitializeComponent();

            AppCenter.Start(
                "ios=1e7f311f-1055-4ec9-8b00-0302015ab8ae;android=6f9a7703-8ca4-477e-9558-7e095f7d20aa;",
                typeof(Analytics),
                typeof(Crashes));

            this.MainPage = new BottomTabbedPage();
        }

        public static ILogger Logger => logger ?? (logger = DependencyService.Get<ILogger>());

        public void SecondOnResume()
        {
            this.OnResume();
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

            try
            {
                if (this.firstRun || Device.RuntimePlatform != Device.iOS)
                {
                    return;
                }

                var mainNav = this.MainPage as NavigationPage;

                var rootPage = mainNav?.CurrentPage as RootPageiOS;

                var rootNav = rootPage?.CurrentPage as NavigationPage;
                if (rootNav == null)
                {
                    return;
                }

                if (rootNav.CurrentPage is AboutPage about)
                {
                    about.OnResume();
                    return;
                }

                if (rootNav.CurrentPage is MeetupPage sessions)
                {
                    sessions.OnResume();
                    return;
                }

                if (rootNav.CurrentPage is NewsPage feed)
                {
                    feed.OnResume();
                }
            }
            catch
            {
                // ignored
            }
            finally
            {
                this.firstRun = false;
            }
        }

        async Task Finish()
        {
            if (Device.RuntimePlatform == Device.iOS && Settings.Current.FirstRun)
            {
#if ENABLE_TEST_CLOUD
                MessagingService.Current.SendMessage<MessagingServiceQuestion>(MessageKeys.Question, new MessagingServiceQuestion
                    {
                        Title = "Push Notifications",
                        Positive = "Let's do it!",
                        Negative = "Maybe Later",
						Question =
$"We can send you updates through {EventInfo.EventName} via push notifications. Would you like to enable them now?",
                        OnCompleted = async (success) =>
                            {
                                if(success)
                                {
                                    var push = DependencyService.Get<IPushNotifications>();
                                    if(push != null)
                                        await push.RegisterForNotifications();
                                }
                            }
                    });
#else
                var push = DependencyService.Get<IPushNotifications>();
                if (push != null)
                {
                    await push.RegisterForNotifications();
                }
#endif
            }
        }

        public new void SendOnAppLinkRequestReceived(Uri uri)
        {
            this.OnAppLinkRequestReceived(uri);
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
            MessagingService.Current.Unsubscribe(MessageKeys.NavigateLogin);
            MessagingService.Current.Unsubscribe<MessagingServiceQuestion>(MessageKeys.Question);
            MessagingService.Current.Unsubscribe<MessagingServiceAlert>(MessageKeys.Message);
            MessagingService.Current.Unsubscribe<MessagingServiceChoice>(MessageKeys.Choice);

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


using System.Globalization;
using XamarinEvolve.Clients.Portable.ApplicationResources;
using XamarinEvolve.Clients.Portable.Interfaces;

namespace XamarinEvolve.Clients.UI
{
    using System;
    using System.Threading.Tasks;

    using FormsToolkit;

    using Plugin.Connectivity;
    using Plugin.Connectivity.Abstractions;

    using Xamarin.Forms;

    using XamarinEvolve.Clients.Portable;
    using XamarinEvolve.Utils.Helpers;

    public static class ViewModelLocator
    {
        private static MeetupViewModel meetupViewModel;

        public static MeetupViewModel MeetupViewModel =>
            meetupViewModel ?? (meetupViewModel = new MeetupViewModel(navigation: null));
    }

    public partial class App : Application
    {
        public static App current;

        public App()
        {
            current = this;
            var ci = Portable.Helpers.Settings.CurrentLanguage == "" ? DependencyService.Get<ILocalize>().GetCurrentCultureInfo() : new CultureInfo(Portable.Helpers.Settings.CurrentLanguage);
            AppResources.Culture = ci;
            ViewModelBase.CurrentLanguage = AppResources.Culture.Name.Substring(0, 2);
            this.InitializeComponent();
            ViewModelBase.Init();
            // The root page of your application
            switch (Device.RuntimePlatform)
            {
                case Device.Android:
                    this.MainPage = new RootPageAndroid();
                    break;
                case Device.iOS:
                    this.MainPage = new EvolveNavigationPage(new RootPageiOS());
                    break;
                case Device.UWP:
                    this.MainPage = new RootPageWindows();
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        static ILogger logger;

        public static ILogger Logger => logger ?? (logger = DependencyService.Get<ILogger>());

        protected override void OnStart()
        {
            this.OnResume();
        }

        public void SecondOnResume()
        {
            this.OnResume();
        }

        bool registered;

        bool firstRun = true;

        protected override void OnResume()
        {
            if (this.registered) return;
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

                        if (task == null) return;

                        await task;
                        info?.OnCompleted?.Invoke();
                    });

            MessagingService.Current.Subscribe<MessagingServiceQuestion>(
                MessageKeys.Question,
                async (m, q) =>
                    {
                        var task = Current?.MainPage?.DisplayAlert(q.Title, q.Question, q.Positive, q.Negative);
                        if (task == null) return;
                        var result = await task;
                        q?.OnCompleted?.Invoke(result);
                    });

            MessagingService.Current.Subscribe<MessagingServiceChoice>(
                MessageKeys.Choice,
                async (m, q) =>
                    {
                        var task = Current?.MainPage?.DisplayActionSheet(q.Title, q.Cancel, q.Destruction, q.Items);
                        if (task == null) return;
                        var result = await task;
                        q?.OnCompleted?.Invoke(result);
                    });

            try
            {
                if (this.firstRun || Device.RuntimePlatform != Device.iOS) return;

                var mainNav = this.MainPage as NavigationPage;
                if (mainNav == null) return;

                var rootPage = mainNav.CurrentPage as RootPageiOS;
                if (rootPage == null) return;

                var rootNav = rootPage.CurrentPage as NavigationPage;
                if (rootNav == null) return;

                var about = rootNav.CurrentPage as AboutPage;
                if (about != null)
                {
                    about.OnResume();
                    return;
                }

                var sessions = rootNav.CurrentPage as MeetupPage;
                if (sessions != null)
                {
                    sessions.OnResume();
                    return;
                }

                var feed = rootNav.CurrentPage as NewsPage;
                if (feed != null)
                {
                    feed.OnResume();
                    return;
                }
            }
            catch
            {
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
                if (push != null) await push.RegisterForNotifications();
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
                && !data.Contains($"/{AboutThisApp.SpeakersSiteSubdirectory.ToLowerInvariant()}/")) return;

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
            if (!this.registered) return;

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


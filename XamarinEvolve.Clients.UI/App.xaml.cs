using System;
using System.Diagnostics;
using System.Threading.Tasks;
using FormsToolkit;
using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;
using Xamarin.Forms;
using XamarinEvolve.Clients.Portable;
using XamarinEvolve.DataStore.Abstractions;
using XamarinEvolve.Utils;

namespace XamarinEvolve.Clients.UI
{
	public partial class App : Application
	{
		public static App current;
		public App()
		{
			current = this;
			InitializeComponent();
			ViewModelBase.Init();
			// The root page of your application
			switch (Device.OS)
			{
				case TargetPlatform.Android:
					MainPage = new RootPageAndroid();
					break;
				case TargetPlatform.iOS:
					MainPage = new EvolveNavigationPage(new RootPageiOS());
					break;
				case TargetPlatform.Windows:
				case TargetPlatform.WinPhone:
					MainPage = new RootPageWindows();
					break;
				default:
					throw new NotImplementedException();
			}
		}

		static ILogger logger;
		public static ILogger Logger => logger ?? (logger = DependencyService.Get<ILogger>());

		protected override void OnStart()
		{
			OnResume();
		}

		public void SecondOnResume()
		{
			OnResume();
		}

		bool registered;
		bool firstRun = true;
		protected override void OnResume()
		{
			Debug.WriteLine($"Current user: {Settings.Current.UserIdentifier}");

			if (registered)
				return;
			registered = true;
			// Handle when your app resumes
			Settings.Current.IsConnected = CrossConnectivity.Current.IsConnected;
			CrossConnectivity.Current.ConnectivityChanged += ConnectivityChanged;

			// Handle when your app starts
			MessagingService.Current.Subscribe<MessagingServiceAlert>(MessageKeys.Message, async (m, info) =>
				{
					var task = Application.Current?.MainPage?.DisplayAlert(info.Title, info.Message, info.Cancel);

					if (task == null)
						return;

					await task;
					info?.OnCompleted?.Invoke();
				});


			MessagingService.Current.Subscribe<MessagingServiceQuestion>(MessageKeys.Question, async (m, q) =>
				{
					var task = Application.Current?.MainPage?.DisplayAlert(q.Title, q.Question, q.Positive, q.Negative);
					if (task == null)
						return;
					var result = await task;
					q?.OnCompleted?.Invoke(result);
				});

			MessagingService.Current.Subscribe<MessagingServiceChoice>(MessageKeys.Choice, async (m, q) =>
				{
					var task = Application.Current?.MainPage?.DisplayActionSheet(q.Title, q.Cancel, q.Destruction, q.Items);
					if (task == null)
						return;
					var result = await task;
					q?.OnCompleted?.Invoke(result);
				});

		    MessagingService.Current.Subscribe(MessageKeys.NavigateLogin, async m =>
		    {
		        MessagingService.Current.SendMessage(MessageKeys.LoggedIn);
		        Logger.Track(EvolveLoggerKeys.LoginSuccess);

		        Settings.Current.FirstRun = false;

		        try
		        {
		            var storeManager = DependencyService.Get<IStoreManager>();
		            await storeManager.SyncAllAsync(Settings.Current.IsLoggedIn);
		            Settings.Current.LastSync = DateTime.UtcNow;
		            Settings.Current.HasSyncedData = true;
		        }
		        catch (Exception ex)
		        {
		            //if sync doesn't work don't worry it is alright we can recover later
		            Logger.Report(ex);
		        }
		        await Finish();
		    });

			try
			{
				if (firstRun || Device.OS != TargetPlatform.iOS)
					return;

				var mainNav = MainPage as NavigationPage;
				if (mainNav == null)
					return;

				var rootPage = mainNav.CurrentPage as RootPageiOS;
				if (rootPage == null)
					return;

				var rootNav = rootPage.CurrentPage as NavigationPage;
				if (rootNav == null)
					return;

				var about = rootNav.CurrentPage as AboutPage;
				if (about != null)
				{
					about.OnResume();
					return;
				}
				var sessions = rootNav.CurrentPage as SessionsPage;
				if (sessions != null)
				{
					sessions.OnResume();
					return;
				}
				var feed = rootNav.CurrentPage as FeedPage;
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
				firstRun = false;
			}
		}

		async Task Finish()
		{
			if (Device.OS == TargetPlatform.iOS && Settings.Current.FirstRun)
			{

#if ENABLE_TEST_CLOUD
                MessagingService.Current.SendMessage<MessagingServiceQuestion>(MessageKeys.Question, new MessagingServiceQuestion
                    {
                        Title = "Push Notifications",
                        Positive = "Let's do it!",
                        Negative = "Maybe Later",
						Question = $"We can send you updates through {EventInfo.EventName} via push notifications. Would you like to enable them now?",
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
					await push.RegisterForNotifications();
#endif
			}
		}

		public void SendOnAppLinkRequestReceived(Uri uri)
		{
			OnAppLinkRequestReceived(uri);
		}

		protected override void OnAppLinkRequestReceived(Uri uri)
		{
			var data = uri.ToString().ToLowerInvariant();

			//only if deep linking
			if (!data.Contains($"/{AboutThisApp.SessionsSiteSubdirectory.ToLowerInvariant()}/") 
			    && !data.Contains($"/{AboutThisApp.SpeakersSiteSubdirectory.ToLowerInvariant()}/"))
				return;

			var id = data.Substring(data.LastIndexOf("/", StringComparison.Ordinal) + 1);

			if (!string.IsNullOrWhiteSpace(id))
			{
				AppPage destination = AppPage.Session;
				if (data.Contains($"/{AboutThisApp.SpeakersSiteSubdirectory.ToLowerInvariant()}/"))
				{
					destination = AppPage.Speaker;
				}
				MessagingService.Current.SendMessage("DeepLinkPage", new DeepLinkPage
				{
					Page = destination,
					Id = id.Replace("#", "")
				});
			}

			base.OnAppLinkRequestReceived(uri);
		}

		protected override void OnSleep()
		{
			if (!registered)
				return;

			registered = false;
			MessagingService.Current.Unsubscribe(MessageKeys.NavigateLogin);
			MessagingService.Current.Unsubscribe<MessagingServiceQuestion>(MessageKeys.Question);
			MessagingService.Current.Unsubscribe<MessagingServiceAlert>(MessageKeys.Message);
			MessagingService.Current.Unsubscribe<MessagingServiceChoice>(MessageKeys.Choice);

			// Handle when your app sleeps
			CrossConnectivity.Current.ConnectivityChanged -= ConnectivityChanged;
		}

		protected void ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
		{
			//save current state and then set it
			var connected = Settings.Current.IsConnected;
			Settings.Current.IsConnected = e.IsConnected;
			if (connected && !e.IsConnected)
			{
				var toaster = DependencyService.Get<IToast>();
				toaster.SendToast("Uh Oh, It looks like you have gone offline. Check your internet connection to get the latest data.");
			}
		}

	}
}


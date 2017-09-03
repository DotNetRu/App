using Xamarin.Forms;
using MvvmHelpers;
using Plugin.Share;
using System.Threading.Tasks;
using System.Windows.Input;
using Plugin.Share.Abstractions;
using System;
using XamarinEvolve.DataStore.Azure.Abstractions;
using XamarinEvolve.DataStore.Azure.Stores;

namespace XamarinEvolve.Clients.Portable
{
	public class ViewModelBase : BaseViewModel
	{
		protected INavigation Navigation { get; }

		public ViewModelBase(INavigation navigation = null)
		{
			Navigation = navigation;
		}

		public static void Init()
		{
			DependencyService.Register<ISessionStore, SessionStore>();
			DependencyService.Register<IFavoriteStore, FavoriteStore>();
			DependencyService.Register<IFeedbackStore, FeedbackStore>();
			DependencyService.Register<IConferenceFeedbackStore, ConferenceFeedbackStore>();
			DependencyService.Register<ISpeakerStore, SpeakerStore>();
			DependencyService.Register<ISponsorStore, SponsorStore>();
			DependencyService.Register<ICategoryStore, CategoryStore>();
			DependencyService.Register<IEventStore, EventStore>();
			DependencyService.Register<INotificationStore, NotificationStore>();
			DependencyService.Register<IStoreManager, DataStore.Azure.StoreManager>();

			DependencyService.Register<FavoriteService>();
		}


		protected ILogger Logger { get; } = DependencyService.Get<ILogger>();
		protected IStoreManager StoreManager { get; } = DependencyService.Get<IStoreManager>();
		protected IToast Toast { get; } = DependencyService.Get<IToast>();

		protected FavoriteService FavoriteService { get; } = DependencyService.Get<FavoriteService>();


		public Settings Settings
		{
			get { return Settings.Current; }
		}

		ICommand launchBrowserCommand;

		public ICommand LaunchBrowserCommand =>
			launchBrowserCommand ?? (launchBrowserCommand =
				new Command<string>(async (t) => await ExecuteLaunchBrowserAsync(t)));

		async Task ExecuteLaunchBrowserAsync(string arg)
		{
			if (IsBusy)
				return;

			if (!arg.StartsWith("http://", StringComparison.OrdinalIgnoreCase) &&
					!arg.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
				arg = "http://" + arg;

			arg = arg.Trim();

			var lower = arg.ToLowerInvariant();

			Logger.Track(EvolveLoggerKeys.LaunchedBrowser, "Url", lower);

			if (Device.OS == TargetPlatform.iOS)
			{
				if (lower.Contains("twitter.com"))
				{
					try
					{
						var id = arg.Substring(lower.LastIndexOf("/", StringComparison.Ordinal) + 1);
						var launchTwitter = DependencyService.Get<ILaunchTwitter>();
						if (lower.Contains("/status/"))
						{
							//status
							if (launchTwitter.OpenStatus(id))
								return;
						}
						else
						{
							//user
							if (launchTwitter.OpenUserName(id))
								return;
						}
					}
					catch
					{
					}
				}
				if (lower.Contains("facebook.com"))
				{
					try
					{
						var id = arg.Substring(lower.LastIndexOf("/", StringComparison.Ordinal) + 1);
						var launchFacebook = DependencyService.Get<ILaunchFacebook>();
						if (launchFacebook.OpenUserName(id))
							return;
					}
					catch
					{
					}
				}
			}

			try
			{
				var primaryColor = ((Color)Application.Current.Resources["Primary"]);

				await CrossShare.Current.OpenBrowser(arg, new BrowserOptions
				{
					ChromeShowTitle = true,
					ChromeToolbarColor = new ShareColor
					{
						A = 255,
						R = Convert.ToInt32(primaryColor.R),
						G = Convert.ToInt32(primaryColor.G),
						B = Convert.ToInt32(primaryColor.B)
					},
					UseSafairReaderMode = true,
					UseSafariWebViewController = true
				});
			}
			catch
			{
			}
		}
	}
}



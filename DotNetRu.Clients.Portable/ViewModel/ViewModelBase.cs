namespace XamarinEvolve.Clients.Portable
{
    using System;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using MvvmHelpers;

    using Plugin.Share;
    using Plugin.Share.Abstractions;

    using Xamarin.Forms;

    using XamarinEvolve.DataStore.Mock.Abstractions;
    using XamarinEvolve.DataStore.Mock.Stores;

    /// <summary>
    /// The view model base.
    /// </summary>
    public class ViewModelBase : BaseViewModel
    {
        /// <summary>
        /// Gets the navigation.
        /// </summary>
        protected INavigation Navigation { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelBase"/> class.
        /// </summary>
        /// <param name="navigation">
        /// The navigation.
        /// </param>
        public ViewModelBase(INavigation navigation = null)
        {
            this.Navigation = navigation;
        }

        /// <summary>
        /// The INIT.
        /// </summary>
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
            DependencyService.Register<IStoreManager, DataStore.Mock.StoreManager>();

            DependencyService.Register<FavoriteService>();
        }

        /// <summary>
        /// Gets the logger.
        /// </summary>
        protected ILogger Logger { get; } = DependencyService.Get<ILogger>();

        /// <summary>
        /// Gets the store manager.
        /// </summary>
        protected IStoreManager StoreManager { get; } = DependencyService.Get<IStoreManager>();

        /// <summary>
        /// Gets the toast.
        /// </summary>
        protected IToast Toast { get; } = DependencyService.Get<IToast>();

        /// <summary>
        /// Gets the favorite service.
        /// </summary>
        protected FavoriteService FavoriteService { get; } = DependencyService.Get<FavoriteService>();

        /// <summary>
        /// The settings.
        /// </summary>
        public Settings Settings => Settings.Current;

        /// <summary>
        /// The launch browser command.
        /// </summary>
        ICommand launchBrowserCommand;

        /// <summary>
        /// The launch browser command.
        /// </summary>
        public ICommand LaunchBrowserCommand => launchBrowserCommand
                                                ?? (launchBrowserCommand = new Command<string>(
                                                        async (t) => await ExecuteLaunchBrowserAsync(t)));

        /// <summary>
        /// The execute launch browser async.
        /// </summary>
        /// <param name="arg">
        /// The arg.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task ExecuteLaunchBrowserAsync(string arg)
        {
            if (this.IsBusy)
            {
                return;
            }

            if (!arg.StartsWith("http://", StringComparison.OrdinalIgnoreCase)
                && !arg.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                arg = "http://" + arg;
            }

            arg = arg.Trim();

            var lower = arg.ToLowerInvariant();

            this.Logger.Track(EvolveLoggerKeys.LaunchedBrowser, "Url", lower);

            if (Device.RuntimePlatform == Device.iOS)
            {
                if (lower.Contains("twitter.com"))
                {
                    try
                    {
                        var id = arg.Substring(lower.LastIndexOf("/", StringComparison.Ordinal) + 1);
                        var launchTwitter = DependencyService.Get<ILaunchTwitter>();
                        if (lower.Contains("/status/"))
                        {
                            // status
                            if (launchTwitter.OpenStatus(id))
                            {
                                return;
                            }
                        }
                        else
                        {
                            // user
                            if (launchTwitter.OpenUserName(id))
                            {
                                return;
                            }
                        }
                    }
                    catch
                    {
                        // ignored
                    }
                }
                if (lower.Contains("facebook.com"))
                {
                    try
                    {
                        var id = arg.Substring(lower.LastIndexOf("/", StringComparison.Ordinal) + 1);
                        var launchFacebook = DependencyService.Get<ILaunchFacebook>();
                        if (launchFacebook.OpenUserName(id))
                        {
                            return;
                        }
                    }
                    catch
                    {
                        // ignored
                    }
                }
            }

            try
            {
                var primaryColor = (Color)Application.Current.Resources["Primary"];

                await CrossShare.Current.OpenBrowser(
                    arg,
                    new BrowserOptions
                        {
                            ChromeShowTitle = true,
                            ChromeToolbarColor =
                                new ShareColor
                                    {
                                        A = 255,
                                        R = Convert.ToInt32(primaryColor.R),
                                        G = Convert.ToInt32(primaryColor.G),
                                        B = Convert.ToInt32(primaryColor.B)
                                    },
                            UseSafariReaderMode = true,
                            UseSafariWebViewController = true
                        });
            }
            catch
            {
                // ignored
            }
        }
    }
}

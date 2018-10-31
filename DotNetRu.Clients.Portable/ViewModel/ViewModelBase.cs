namespace DotNetRu.Clients.Portable.ViewModel
{
    using System;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using DotNetRu.Clients.Portable.Helpers;
    using DotNetRu.Clients.Portable.Interfaces;
    using DotNetRu.Utils.Interfaces;

    using MvvmHelpers;

    using Plugin.Share;
    using Plugin.Share.Abstractions;

    using Xamarin.Forms;

    using Settings = DotNetRu.Utils.Helpers.Settings;

    public class ViewModelBase : BaseViewModel
    {
        private ICommand launchBrowserCommand;

        public ViewModelBase(INavigation navigation = null)
        {
            this.Navigation = navigation;

            this.Resources = new LocalizedResources();
        }

        public LocalizedResources Resources
        {
            get;
        }

        public Settings Settings => Settings.Current;

        public ICommand LaunchBrowserCommand => this.launchBrowserCommand
                                                ?? (this.launchBrowserCommand = new Command<string>(
                                                        async (t) => await this.ExecuteLaunchBrowserAsync(t)));

        protected INavigation Navigation { get; }

        protected ILogger Logger { get; } = DependencyService.Get<ILogger>();

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

            this.Logger.Track(DotNetRuLoggerKeys.LaunchedBrowser, "Url", lower);

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

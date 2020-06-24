namespace DotNetRu.Clients.Portable.ViewModel
{
    using System;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using DotNetRu.Clients.Portable.Helpers;
    using DotNetRu.Clients.Portable.Interfaces;
    using DotNetRu.Utils.Interfaces;

    using MvvmHelpers;
    using Xamarin.Essentials;
    using Xamarin.Forms;

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

        public ICommand LaunchBrowserCommand => this.launchBrowserCommand ??= new Command<Uri>(
            async (t) => await this.ExecuteLaunchBrowserAsync(t));

        protected internal INavigation Navigation { get; }

        protected ILogger Logger { get; } = DependencyService.Get<ILogger>();

        public async Task ExecuteLaunchBrowserAsync(Uri link)
        {
            if (this.IsBusy)
            {
                return;
            }

            var stringLink = link.ToString();

            this.Logger.Track(DotNetRuLoggerKeys.LaunchedBrowser, "Url", stringLink);

            if (Device.RuntimePlatform == Device.iOS)
            {
                if (stringLink.Contains("twitter.com"))
                {
                    try
                    {
                        var id = stringLink.Substring(stringLink.LastIndexOf("/", StringComparison.Ordinal) + 1);
                        var launchTwitter = DependencyService.Get<ILaunchTwitter>();
                        if (stringLink.Contains("/status/"))
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

                if (stringLink.Contains("facebook.com"))
                {
                    try
                    {
                        var id = stringLink.Substring(stringLink.LastIndexOf("/", StringComparison.Ordinal) + 1);
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
                await Browser.OpenAsync(link, BrowserLaunchMode.SystemPreferred);
            }
            catch
            {
                // ignored
            }
        }
    }
}

namespace XamarinEvolve.Clients.Portable
{
    using MvvmHelpers;

    using Xamarin.Forms;

    using XamarinEvolve.Utils;
    using XamarinEvolve.Utils.Helpers;

    public class SettingsViewModel : ViewModelBase
    {
        public ObservableRangeCollection<MenuItem> AboutItems { get; } = new ObservableRangeCollection<MenuItem>();

        public ObservableRangeCollection<MenuItem> TechnologyItems { get; } = new ObservableRangeCollection<MenuItem>();

        public string Copyright => AboutThisApp.Copyright;

        public bool AppToWebLinkingEnabled => FeatureFlags.AppToWebLinkingEnabled;

        public SettingsViewModel()
        {
            this.AboutItems.AddRange(
                new[]
                    {
                        new MenuItem
                            {
                                Name = $"Created by {AboutThisApp.Developer}",
                                Command = this.LaunchBrowserCommand,
                                Parameter = AboutThisApp.DeveloperWebsite
                            },
                        new MenuItem
                            {
                                Name = $"Big thanks to James Montemagno!",
                                Command = this.LaunchBrowserCommand,
                                Parameter = AboutThisApp.MontemagnoWebsite
                            },
                        new MenuItem
                            {
                                Name = "Open source on GitHub!",
                                Command = this.LaunchBrowserCommand,
                                Parameter = AboutThisApp.OpenSourceUrl
                            },
                        new MenuItem
                            {
                                Name = "Open Source Notice",
                                Command = this.LaunchBrowserCommand,
                                Parameter = AboutThisApp.OpenSourceNoticeUrl
                            },
                        new MenuItem
                            {
                                Name = "Issue tracker",
                                Command = this.LaunchBrowserCommand,
                                Parameter = AboutThisApp.OpenSourceUrl
                            }
                    });

            this.TechnologyItems.AddRange(
                new[]
                    {
                        new MenuItem
                            {
                                Name = "Censored",
                                Command = this.LaunchBrowserCommand,
                                Parameter = "https://github.com/jamesmontemagno/Censored"
                            },
                        new MenuItem
                            {
                                Name = "Connectivity Plugin",
                                Command = this.LaunchBrowserCommand,
                                Parameter =
                                    "https://github.com/jamesmontemagno/Xamarin.Plugins/tree/master/Connectivity"
                            },
                        new MenuItem
                            {
                                Name = "Humanizer",
                                Command = this.LaunchBrowserCommand,
                                Parameter = "https://github.com/Humanizr/Humanizer"
                            },
                        new MenuItem
                            {
                                Name = "Image Circles",
                                Command = this.LaunchBrowserCommand,
                                Parameter =
                                    "https://github.com/jamesmontemagno/Xamarin.Plugins/tree/master/ImageCircle"
                            },
                        new MenuItem
                            {
                                Name = "LinqToTwitter",
                                Command = this.LaunchBrowserCommand,
                                Parameter = "https://github.com/JoeMayo/LinqToTwitter"
                            },
                        new MenuItem
                            {
                                Name = "Messaging Plugin",
                                Command = this.LaunchBrowserCommand,
                                Parameter = "https://github.com/cjlotz/Xamarin.Plugins"
                            },
                        new MenuItem
                            {
                                Name = "Mvvm Helpers",
                                Command = this.LaunchBrowserCommand,
                                Parameter = "https://github.com/jamesmontemagno/mvvm-helpers"
                            },
                        new MenuItem
                            {
                                Name = "Noda Time",
                                Command = this.LaunchBrowserCommand,
                                Parameter = "https://github.com/nodatime/nodatime"
                            },
                        new MenuItem
                            {
                                Name = "Permissions Plugin",
                                Command = this.LaunchBrowserCommand,
                                Parameter =
                                    "https://github.com/jamesmontemagno/Xamarin.Plugins/tree/master/Permissions"
                            },
                        new MenuItem
                            {
                                Name = "Pull to Refresh Layout",
                                Command = this.LaunchBrowserCommand,
                                Parameter =
                                    "https://github.com/jamesmontemagno/Xamarin.Forms-PullToRefreshLayout"
                            },
                        new MenuItem
                            {
                                Name = "Settings Plugin",
                                Command = this.LaunchBrowserCommand,
                                Parameter =
                                    "https://github.com/jamesmontemagno/Xamarin.Plugins/tree/master/Settings"
                            },
                        new MenuItem
                            {
                                Name = "Toolkit for Xamarin.Forms",
                                Command = this.LaunchBrowserCommand,
                                Parameter = "https://github.com/jamesmontemagno/xamarin.forms-toolkit"
                            },
                        new MenuItem
                            {
                                Name = "Xamarin.Forms",
                                Command = this.LaunchBrowserCommand,
                                Parameter = "http://xamarin.com/forms"
                            },
                        new MenuItem
                            {
                                Name = "Xamarin Insights",
                                Command = this.LaunchBrowserCommand,
                                Parameter = "http://xamarin.com/insights"
                            }
                    });
        }

        public string AppVersion => $"Version {DependencyService.Get<IAppVersionProvider>()?.AppVersion ?? "1.0"}";

        public string AppInfo => "Всероссийское .NET сообщество\nDotNetRu - группа независимых сообществ .NET разработчиков со всей России. Мы объединяем людей вокруг .NET платформы, чтобы способствовать обмену опытом и знаниями. Проводим регулярные встречи, чтобы делиться новостями и лучшими практиками в разработке программных продуктов.";
    }
}

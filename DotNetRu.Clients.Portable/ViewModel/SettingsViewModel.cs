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
            //This will be triggered wen 
            Settings.PropertyChanged += async (sender, e) =>
                {
                    if (e.PropertyName == "Email")
                    {
                        OnPropertyChanged("LoginText");
                        OnPropertyChanged("AccountItems");
                    }
                };

            AboutItems.AddRange(
                new[]
                    {
                        new MenuItem
                            {
                                Name = $"Created by {AboutThisApp.Developer} with <3",
                                Command = LaunchBrowserCommand,
                                Parameter = AboutThisApp.DeveloperWebsite
                            },
                        new MenuItem
                        {
                            Name = $"Big thanks to James Montemagno!",
                            Command = LaunchBrowserCommand,
                            Parameter = AboutThisApp.MontemagnoWebsite
                        },
                        new MenuItem
                            {
                                Name = "Open source on GitHub!",
                                Command = LaunchBrowserCommand,
                                Parameter = AboutThisApp.OpenSourceUrl
                            },
                        new MenuItem
                            {
                                Name = "Terms of Use",
                                Command = LaunchBrowserCommand,
                                Parameter = AboutThisApp.TermsOfUseUrl
                            },
                        new MenuItem
                            {
                                Name = "Privacy Policy",
                                Command = LaunchBrowserCommand,
                                Parameter = AboutThisApp.PrivacyPolicyUrl
                            },
                        new MenuItem
                            {
                                Name = "Open Source Notice",
                                Command = LaunchBrowserCommand,
                                Parameter = AboutThisApp.OpenSourceNoticeUrl
                            }
                    });

            TechnologyItems.AddRange(
                new[]
                    {
                        new MenuItem
                            {
                                Name = "Censored",
                                Command = LaunchBrowserCommand,
                                Parameter = "https://github.com/jamesmontemagno/Censored"
                            },
                        new MenuItem
                            {
                                Name = "Calendar Plugin",
                                Command = LaunchBrowserCommand,
                                Parameter = "https://github.com/TheAlmightyBob/Calendars"
                            },
                        new MenuItem
                            {
                                Name = "Connectivity Plugin",
                                Command = LaunchBrowserCommand,
                                Parameter =
                                    "https://github.com/jamesmontemagno/Xamarin.Plugins/tree/master/Connectivity"
                            },
                        new MenuItem
                            {
                                Name = "Embedded Resource Plugin",
                                Command = LaunchBrowserCommand,
                                Parameter = "https://github.com/JosephHill/EmbeddedResourcePlugin"
                            },
                        new MenuItem
                            {
                                Name = "Humanizer",
                                Command = LaunchBrowserCommand,
                                Parameter = "https://github.com/Humanizr/Humanizer"
                            },
                        new MenuItem
                            {
                                Name = "Image Circles",
                                Command = LaunchBrowserCommand,
                                Parameter =
                                    "https://github.com/jamesmontemagno/Xamarin.Plugins/tree/master/ImageCircle"
                            },
                        new MenuItem
                            {
                                Name = "Json.NET",
                                Command = LaunchBrowserCommand,
                                Parameter = "https://github.com/JamesNK/Newtonsoft.Json"
                            },
                        new MenuItem
                            {
                                Name = "LinqToTwitter",
                                Command = LaunchBrowserCommand,
                                Parameter = "https://github.com/JoeMayo/LinqToTwitter"
                            },
                        new MenuItem
                            {
                                Name = "Messaging Plugin",
                                Command = LaunchBrowserCommand,
                                Parameter = "https://github.com/cjlotz/Xamarin.Plugins"
                            },
                        new MenuItem
                            {
                                Name = "Mvvm Helpers",
                                Command = LaunchBrowserCommand,
                                Parameter = "https://github.com/jamesmontemagno/mvvm-helpers"
                            },
                        new MenuItem
                            {
                                Name = "Noda Time",
                                Command = LaunchBrowserCommand,
                                Parameter = "https://github.com/nodatime/nodatime"
                            },
                        new MenuItem
                            {
                                Name = "Permissions Plugin",
                                Command = LaunchBrowserCommand,
                                Parameter =
                                    "https://github.com/jamesmontemagno/Xamarin.Plugins/tree/master/Permissions"
                            },
                        new MenuItem
                            {
                                Name = "PCL Storage",
                                Command = LaunchBrowserCommand,
                                Parameter = "https://github.com/dsplaisted/PCLStorage"
                            },
                        new MenuItem
                            {
                                Name = "Pull to Refresh Layout",
                                Command = LaunchBrowserCommand,
                                Parameter =
                                    "https://github.com/jamesmontemagno/Xamarin.Forms-PullToRefreshLayout"
                            },
                        new MenuItem
                            {
                                Name = "Settings Plugin",
                                Command = LaunchBrowserCommand,
                                Parameter =
                                    "https://github.com/jamesmontemagno/Xamarin.Plugins/tree/master/Settings"
                            },
                        new MenuItem
                            {
                                Name = "Toolkit for Xamarin.Forms",
                                Command = LaunchBrowserCommand,
                                Parameter = "https://github.com/jamesmontemagno/xamarin.forms-toolkit"
                            },
                        new MenuItem
                            {
                                Name = "Xamarin.Forms",
                                Command = LaunchBrowserCommand,
                                Parameter = "http://xamarin.com/forms"
                            },
                        new MenuItem
                            {
                                Name = "Xamarin Insights",
                                Command = LaunchBrowserCommand,
                                Parameter = "http://xamarin.com/insights"
                            }
                    });
        }

        public string AppVersion
        {
            get
            {
                return $"Version {DependencyService.Get<IAppVersionProvider>()?.AppVersion ?? "1.0"}";
            }
        }

#if DEBUG
        public bool IsDebug => true;
#else
		public bool IsDebug => false;
#endif
    }
}

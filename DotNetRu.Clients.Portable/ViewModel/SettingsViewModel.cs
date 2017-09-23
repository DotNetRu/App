namespace XamarinEvolve.Clients.Portable
{
    using System;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using Humanizer;

    using MvvmHelpers;

    using Plugin.Connectivity;

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
                        Settings.NeedsSync = true;
                        OnPropertyChanged("LoginText");
                        OnPropertyChanged("AccountItems");
                        //if logged in you should go ahead and sync data.
                        if (Settings.IsLoggedIn)
                        {
                            await ExecuteSyncCommandAsync();
                        }
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
                                Name = "Azure Mobile Apps",
                                Command = LaunchBrowserCommand,
                                Parameter = "https://github.com/Azure/azure-mobile-apps-net-client/"
                            },
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
                                Name = "External Maps Plugin",
                                Command = LaunchBrowserCommand,
                                Parameter =
                                    "https://github.com/jamesmontemagno/Xamarin.Plugins/tree/master/ExternalMaps"
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
                            },
                        new MenuItem
                            {
                                Name = "ZXing.Net Mobile",
                                Command = LaunchBrowserCommand,
                                Parameter = "https://github.com/Redth/ZXing.Net.Mobile"
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

        public string LastSyncDisplay
        {
            get
            {
                if (!Settings.HasSyncedData) return "Never";

                return Settings.LastSync.Humanize();
            }
        }

#if DEBUG
        public bool IsDebug => true;
#else
		public bool IsDebug => false;
#endif

        ICommand forceResetCommand;

        public ICommand ForceResetCommand => forceResetCommand
                                             ?? (forceResetCommand = new Command(
                                                     async () => await ExecuteForceResetCommandAsync()));

        async Task ExecuteForceResetCommandAsync()
        {
            var userId = Settings.UserIdentifier;
        }

        static readonly string defaultSyncText = Device.OS == TargetPlatform.iOS ? "Sync Now" : "Last Sync";

        string syncText = defaultSyncText;

        public string SyncText
        {
            get
            {
                return syncText;
            }
            set
            {
                SetProperty(ref syncText, value);
            }
        }

        ICommand syncCommand;

        public ICommand SyncCommand =>
            syncCommand ?? (syncCommand = new Command(async () => await ExecuteSyncCommandAsync()));

        async Task ExecuteSyncCommandAsync()
        {

            if (IsBusy) return;

            if (!CrossConnectivity.Current.IsConnected)
            {
                MessagingUtils.SendOfflineMessage();
                return;
            }

            Logger.Track(EvolveLoggerKeys.ManualSync);

            SyncText = "Syncing...";
            IsBusy = true;

            try
            {
#if DEBUG
                await Task.Delay(1000);
#endif

                Settings.HasSyncedData = true;
                Settings.LastSync = DateTime.UtcNow;
                OnPropertyChanged("LastSyncDisplay");

                await StoreManager.SyncAllAsync(Settings.Current.IsLoggedIn);
            }
            catch (Exception ex)
            {
                ex.Data["method"] = "ExecuteSyncCommandAsync";
                MessagingUtils.SendAlert(
                    "Unable to sync",
                    "Uh oh, something went wrong with the sync, please try again. \n\n Debug:" + ex.Message);
                Logger.Report(ex);
            }
            finally
            {
                SyncText = defaultSyncText;
                IsBusy = false;
            }
        }
    }
}

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Globalization;
using XamarinEvolve.Clients.Portable.ApplicationResources;
using XamarinEvolve.Clients.Portable.Helpers;
using XamarinEvolve.Clients.Portable.Interfaces;
using XamarinEvolve.Clients.UI;

namespace XamarinEvolve.Clients.Portable
{
    using MvvmHelpers;
    using Xamarin.Forms;
    using XamarinEvolve.Utils;
    using XamarinEvolve.Utils.Helpers;
    public class CustomObservableCollection<T> : ObservableRangeCollection<T>
    {
        public CustomObservableCollection() { }
        public CustomObservableCollection(IEnumerable<T> items) : this()
        {
            foreach (var item in items)
                this.Add(item);
        }
        public void ReportItemChange(T item)
        {
            NotifyCollectionChangedEventArgs args =
                new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Replace,
                    item,
                    item,
                    IndexOf(item));
            OnCollectionChanged(args);
        }
    }

    public class SettingsViewModel : ViewModelBase
    {

        public List<string> Languages { get; set; } = new List<string>()
        {
            "en",
            "ru",
        };

        private string _SelectedLanguage;

        public string SelectedLanguage
        {
            get { return _SelectedLanguage; }
            set
            {
                _SelectedLanguage = value;
                SetLanguage();
            }
        }

        private void SetLanguage()
        {
            CurrentLanguage = SelectedLanguage;
            MessagingCenter.Send<object, CultureChangedMessage>(this,
                string.Empty, new CultureChangedMessage(SelectedLanguage));
        }

        public CustomObservableCollection<MenuItem> AboutItems { get; } = new CustomObservableCollection<MenuItem>();

        public ObservableRangeCollection<MenuItem> TechnologyItems { get; } = new ObservableRangeCollection<MenuItem>();

        public string Copyright => AboutThisApp.Copyright;

        public bool AppToWebLinkingEnabled => FeatureFlags.AppToWebLinkingEnabled;

        void NotifyVmProperties()
        {
            var ci = new CultureInfo(CurrentLanguage);
            DependencyService.Get<ILocalize>().SetLocale(ci); // set the Thread for locale-aware methods
            AppResources.Culture = ci;
            OnPropertyChanged(nameof(AppInfo));
            OnPropertyChanged(nameof(AppVersion));
            this.AboutItems.ReplaceRange(
                new[]
                {
                    new MenuItem
                    {
                        Name = Resources["CreatedBy"],
                        Command = this.LaunchBrowserCommand,
                        Parameter = AboutThisApp.DeveloperWebsite
                    },
                    new MenuItem
                    {
                        Name = Resources["Thanks"],
                        Command = this.LaunchBrowserCommand,
                        Parameter = AboutThisApp.MontemagnoWebsite
                    },
                    new MenuItem
                    {
                        Name = Resources["OpenSource"],
                        Command = this.LaunchBrowserCommand,
                        Parameter = AboutThisApp.OpenSourceUrl
                    },
                    new MenuItem
                    {
                        Name = Resources["TermsOfUse"],
                        Command = this.LaunchBrowserCommand,
                        Parameter = AboutThisApp.TermsOfUseUrl
                    },
                    new MenuItem
                    {
                        Name = Resources["PrivacyPolicy"],
                        Command = this.LaunchBrowserCommand,
                        Parameter = AboutThisApp.PrivacyPolicyUrl
                    },
                    new MenuItem
                    {
                        Name = Resources["OpenSourceNotice"],
                        Command = this.LaunchBrowserCommand,
                        Parameter = AboutThisApp.OpenSourceNoticeUrl
                    }
                });
        }
        public SettingsViewModel()
        {
            MessagingCenter.Subscribe<LocalizedResources>(this, MessageKeys.LanguageChanged, sender => NotifyVmProperties());
            _SelectedLanguage = CurrentLanguage;
            this.AboutItems.AddRange(
                new[]
                {
                    new MenuItem
                    {
                        Name = Resources["CreatedBy"],
                        Command = this.LaunchBrowserCommand,
                        Parameter = AboutThisApp.DeveloperWebsite
                    },
                    new MenuItem
                    {
                        Name = Resources["Thanks"],
                        Command = this.LaunchBrowserCommand,
                        Parameter = AboutThisApp.MontemagnoWebsite
                    },
                    new MenuItem
                    {
                        Name = Resources["OpenSource"],
                        Command = this.LaunchBrowserCommand,
                        Parameter = AboutThisApp.OpenSourceUrl
                    },
                    new MenuItem
                    {
                        Name = Resources["TermsOfUse"],
                        Command = this.LaunchBrowserCommand,
                        Parameter = AboutThisApp.TermsOfUseUrl
                    },
                    new MenuItem
                    {
                        Name = Resources["PrivacyPolicy"],
                        Command = this.LaunchBrowserCommand,
                        Parameter = AboutThisApp.PrivacyPolicyUrl
                    },
                    new MenuItem
                    {
                        Name = Resources["OpenSourceNotice"],
                        Command = this.LaunchBrowserCommand,
                        Parameter = AboutThisApp.OpenSourceNoticeUrl
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
                        Name = "Calendar Plugin",
                        Command = this.LaunchBrowserCommand,
                        Parameter = "https://github.com/TheAlmightyBob/Calendars"
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
                        Name = "Embedded Resource Plugin",
                        Command = this.LaunchBrowserCommand,
                        Parameter = "https://github.com/JosephHill/EmbeddedResourcePlugin"
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
                        Name = "Json.NET",
                        Command = this.LaunchBrowserCommand,
                        Parameter = "https://github.com/JamesNK/Newtonsoft.Json"
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
                        Name = "PCL Storage",
                        Command = this.LaunchBrowserCommand,
                        Parameter = "https://github.com/dsplaisted/PCLStorage"
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

        public string AppVersion => $"{Resources["Version"]} {DependencyService.Get<IAppVersionProvider>()?.AppVersion ?? "1.0"}";

        public string AppInfo =>
            Resources["AboutText"];

#if DEBUG
        public bool IsDebug => true;
#else
		public bool IsDebug => false;
#endif
    }
}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
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

        private readonly Dictionary<string, string> _languageCode = new Dictionary<string, string>
        {
            ["English"] = "en",
            ["Русский"] = "ru"
        };

        public List<string> Languages => _languageCode.Keys.ToList();

        private string _selectedLanguage;

        public string SelectedLanguage
        {
            get { return _selectedLanguage; }
            set
            {
                SetProperty(ref _selectedLanguage, value);
                SetLanguage();
            }
        }

        private void SetLanguage()
        {
            Helpers.Settings.CurrentLanguage = _languageCode[_selectedLanguage];
            CurrentLanguage = _languageCode[_selectedLanguage];
            MessagingCenter.Send<object, CultureChangedMessage>(this,
                string.Empty, new CultureChangedMessage(_languageCode[_selectedLanguage]));
        }

        public CustomObservableCollection<MenuItem> AboutItems { get; } = new CustomObservableCollection<MenuItem>();

        public CustomObservableCollection<MenuItem> Communities { get; } = new CustomObservableCollection<MenuItem>();

        public string Copyright => AboutThisApp.Copyright;

        public bool AppToWebLinkingEnabled => FeatureFlags.AppToWebLinkingEnabled;

        public ImageSource BackgroundImage { get; set; }
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
                        Name = Resources["IssueTracker"],
                        Command = this.LaunchBrowserCommand,
                        Parameter = AboutThisApp.IssueTracker
                    }
                });
            this.Communities.ReplaceRange(
                new[]
                {
                    new MenuItem
                    {
                        Name = Resources["SaintPetersburg"],
                        Command = this.LaunchBrowserCommand,
                        IconSource = ImageSource.FromUri(new Uri(AboutThisApp.SpbLogo)),
                        Parameter = AboutThisApp.SpbLink
                    },
                    new MenuItem
                    {
                        Name = Resources["Krasnoyarsk"],
                        Command = this.LaunchBrowserCommand,
                        IconSource = ImageSource.FromUri(new Uri(AboutThisApp.KrasnoyarskLogo)),
                        Parameter = AboutThisApp.KrasnoyarskLink
                    },
                    new MenuItem
                    {
                        Name = Resources["Saratov"],
                        Command = this.LaunchBrowserCommand,
                        IconSource = ImageSource.FromUri(new Uri(AboutThisApp.SaratovLogo)),
                        Parameter = AboutThisApp.SaratovLink
                    },
                    new MenuItem
                    {
                        Name = Resources["Moscow"],
                        Command = this.LaunchBrowserCommand,
                        IconSource = ImageSource.FromUri(new Uri(AboutThisApp.MoscowLogo)),
                        Parameter = AboutThisApp.MoscowLink
                    }
                });
        }
        public SettingsViewModel()
        {
            MessagingCenter.Subscribe<LocalizedResources>(this, MessageKeys.LanguageChanged, sender => NotifyVmProperties());
            _selectedLanguage = _languageCode.FirstOrDefault(l => l.Value == Helpers.Settings.CurrentLanguage).Value;
            if (string.IsNullOrEmpty(_selectedLanguage))
                _selectedLanguage = "English";
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
                        Name = Resources["IssueTracker"],
                        Command = this.LaunchBrowserCommand,
                        Parameter = AboutThisApp.IssueTracker
                    }
                });

            this.Communities.AddRange(
                new[]
                {
                    new MenuItem
                    {
                        Name = Resources["SaintPetersburg"],
                        Command = this.LaunchBrowserCommand,
                        IconSource = ImageSource.FromUri(new Uri(AboutThisApp.SpbLogo)),
                        Parameter = AboutThisApp.SpbLink
                    },
                    new MenuItem
                    {
                        Name = Resources["Krasnoyarsk"],
                        Command = this.LaunchBrowserCommand,
                        IconSource = ImageSource.FromUri(new Uri(AboutThisApp.KrasnoyarskLogo)),
                        Parameter = AboutThisApp.KrasnoyarskLink
                    },
                    new MenuItem
                    {
                        Name = Resources["Saratov"],
                        Command = this.LaunchBrowserCommand,
                        IconSource = ImageSource.FromUri(new Uri(AboutThisApp.SaratovLogo)),
                        Parameter = AboutThisApp.SaratovLink
                    },
                    new MenuItem
                    {
                        Name = Resources["Moscow"],
                        Command = this.LaunchBrowserCommand,
                        IconSource = ImageSource.FromUri(new Uri(AboutThisApp.MoscowLogo)),
                        Parameter = AboutThisApp.MoscowLink
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
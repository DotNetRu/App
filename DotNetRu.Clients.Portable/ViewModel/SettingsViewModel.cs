﻿namespace XamarinEvolve.Clients.Portable
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using Xamarin.Forms;

    using XamarinEvolve.Clients.Portable.ApplicationResources;
    using XamarinEvolve.Clients.Portable.Interfaces;
    using XamarinEvolve.Clients.UI;
    using XamarinEvolve.Utils;
    using XamarinEvolve.Utils.Helpers;

    public class SettingsViewModel : ViewModelBase
    {
        private Language selectedLanguage;

        public SettingsViewModel()
        {
            MessagingCenter.Subscribe<LocalizedResources>(
                this,
                MessageKeys.LanguageChanged,
                sender => this.NotifyViewModel());

            this.selectedLanguage = Helpers.Settings.CurrentLanguage ?? Language.English;

            this.AboutItems.AddRange(
                new[]
                    {
                        new MenuItem
                            {
                                Name = this.Resources["CreatedBy"],
                                Command = this.LaunchBrowserCommand,
                                Parameter = AboutThisApp.DeveloperWebsite
                            },
                        new MenuItem
                            {
                                Name = this.Resources["Thanks"],
                                Command = this.LaunchBrowserCommand,
                                Parameter = AboutThisApp.MontemagnoWebsite
                            },
                        new MenuItem
                            {
                                Name = this.Resources["IssueTracker"],
                                Command = this.LaunchBrowserCommand,
                                Parameter = AboutThisApp.IssueTracker
                            }
                    });

            this.Communities.AddRange(
                new[]
                    {
                        new MenuItem
                            {
                                Name = this.Resources["SaintPetersburg"],
                                Command = this.LaunchBrowserCommand,
                                IconSource = ImageSource.FromUri(new Uri(AboutThisApp.SpbLogo)),
                                Parameter = AboutThisApp.SpbLink
                            },
                        new MenuItem
                            {
                                Name = this.Resources["Krasnoyarsk"],
                                Command = this.LaunchBrowserCommand,
                                IconSource = ImageSource.FromUri(new Uri(AboutThisApp.KrasnoyarskLogo)),
                                Parameter = AboutThisApp.KrasnoyarskLink
                            },
                        new MenuItem
                            {
                                Name = this.Resources["Saratov"],
                                Command = this.LaunchBrowserCommand,
                                IconSource = ImageSource.FromUri(new Uri(AboutThisApp.SaratovLogo)),
                                Parameter = AboutThisApp.SaratovLink
                            },
                        new MenuItem
                            {
                                Name = this.Resources["Moscow"],
                                Command = this.LaunchBrowserCommand,
                                IconSource = ImageSource.FromUri(new Uri(AboutThisApp.MoscowLogo)),
                                Parameter = AboutThisApp.MoscowLink
                            }
                    });
        }

        public IEnumerable<string> LanguageNames => EnumExtension.GetEnumValues<Language>().Select(x => x.GetDisplayName());

        public Language SelectedLanguage
        {
            get => this.selectedLanguage;
            set
            {
                this.SetProperty(ref this.selectedLanguage, value);

                Helpers.Settings.CurrentLanguage = this.selectedLanguage;
                ViewModelBase.CurrentLanguage = this.selectedLanguage;
                MessagingCenter.Send<object, CultureChangedMessage>(
                    this,
                    string.Empty,
                    new CultureChangedMessage(this.selectedLanguage.GetLanguageCode()));
            }
        }

        public CustomObservableCollection<MenuItem> AboutItems { get; } = new CustomObservableCollection<MenuItem>();

        public CustomObservableCollection<MenuItem> Communities { get; } = new CustomObservableCollection<MenuItem>();

        public string Copyright => AboutThisApp.Copyright;

        public bool AppToWebLinkingEnabled => FeatureFlags.AppToWebLinkingEnabled;

        public ImageSource BackgroundImage { get; set; }

        public string AppVersion =>
            $"{this.Resources["Version"]} {DependencyService.Get<IAppVersionProvider>()?.AppVersion ?? "1.0"}";

        public string AppInfo => this.Resources["AboutText"];

        private void NotifyViewModel()
        {
            var ci = new CultureInfo(CurrentLanguage.GetLanguageCode());
            DependencyService.Get<ILocalize>().SetLocale(ci); // set the Thread for locale-aware methods
            AppResources.Culture = ci;

            this.OnPropertyChanged(nameof(this.AppInfo));
            this.OnPropertyChanged(nameof(this.AppVersion));

            this.AboutItems.ReplaceRange(
                new[]
                    {
                        new MenuItem
                            {
                                Name = this.Resources["CreatedBy"],
                                Command = this.LaunchBrowserCommand,
                                Parameter = AboutThisApp.DeveloperWebsite
                            },
                        new MenuItem
                            {
                                Name = this.Resources["Thanks"],
                                Command = this.LaunchBrowserCommand,
                                Parameter = AboutThisApp.MontemagnoWebsite
                            },
                        new MenuItem
                            {
                                Name = this.Resources["IssueTracker"],
                                Command = this.LaunchBrowserCommand,
                                Parameter = AboutThisApp.IssueTracker
                            }
                    });
            this.Communities.ReplaceRange(
                new[]
                    {
                        new MenuItem
                            {
                                Name = this.Resources["SaintPetersburg"],
                                Command = this.LaunchBrowserCommand,
                                IconSource = ImageSource.FromUri(new Uri(AboutThisApp.SpbLogo)),
                                Parameter = AboutThisApp.SpbLink
                            },
                        new MenuItem
                            {
                                Name = this.Resources["Krasnoyarsk"],
                                Command = this.LaunchBrowserCommand,
                                IconSource = ImageSource.FromUri(new Uri(AboutThisApp.KrasnoyarskLogo)),
                                Parameter = AboutThisApp.KrasnoyarskLink
                            },
                        new MenuItem
                            {
                                Name = this.Resources["Saratov"],
                                Command = this.LaunchBrowserCommand,
                                IconSource = ImageSource.FromUri(new Uri(AboutThisApp.SaratovLogo)),
                                Parameter = AboutThisApp.SaratovLink
                            },
                        new MenuItem
                            {
                                Name = this.Resources["Moscow"],
                                Command = this.LaunchBrowserCommand,
                                IconSource = ImageSource.FromUri(new Uri(AboutThisApp.MoscowLogo)),
                                Parameter = AboutThisApp.MoscowLink
                            }
                    });
        }
    }
}
namespace DotNetRu.Clients.Portable.ViewModel
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Windows.Input;

    using DotNetRu.Clients.Portable.ApplicationResources;
    using DotNetRu.Clients.Portable.Helpers;
    using DotNetRu.Clients.Portable.Interfaces;
    using DotNetRu.Clients.Portable.Model;
    using DotNetRu.DataStore.Audit.Services;
    using DotNetRu.Utils.Helpers;

    using Xamarin.Forms;
    using Xamarin.Forms.Internals;

    public class SettingsViewModel : ViewModelBase
    {
        private Language selectedLanguage;

        public SettingsViewModel(ICommand openTechologiesUsedCommand)
        {
            MessagingCenter.Subscribe<LocalizedResources>(
                this,
                MessageKeys.LanguageChanged,
                sender => this.NotifyViewModel());

            var savedLanguage = XamarinEvolve.Clients.Portable.Helpers.Settings.CurrentLanguage;
            var uiLanguage = DependencyService.Get<ILocalize>().GetCurrentCultureInfo().TwoLetterISOLanguageName == "ru"
                                 ? Language.Russian
                                 : Language.English;

            this.selectedLanguage = savedLanguage ?? uiLanguage;

            this.AboutItems.AddRange(
                new[]
                    {
                        new LocalizableMenuItem
                            {
                                ResourceName = "CreatedBy",
                                Command = this.LaunchBrowserCommand,
                                Parameter = AboutThisApp.DeveloperWebsite
                            },
                        new LocalizableMenuItem
                            {
                                ResourceName = "Thanks",
                                Command = this.LaunchBrowserCommand,
                                Parameter = AboutThisApp.MontemagnoWebsite
                            },
                        new LocalizableMenuItem
                            {
                                ResourceName = "IssueTracker",
                                Command = this.LaunchBrowserCommand,
                                Parameter = AboutThisApp.IssueTracker
                            },
                        new LocalizableMenuItem
                            {
                                ResourceName = "TechnologyUsed",
                                Command = openTechologiesUsedCommand
                            }
                    });

            this.Communities.AddRange(
                new[]
                    {
                        new LocalizableMenuItem
                            {
                                ResourceName = "DotNetRu",
                                Command = this.LaunchBrowserCommand,
                                ImageSource = LogoService.DotNetRuLogo,
                                Parameter = AboutThisApp.DotNetRuLink
                            }, 
                        new LocalizableMenuItem
                            {
                                ResourceName = "SaintPetersburg",
                                Command = this.LaunchBrowserCommand,
                                ImageSource = LogoService.SpbDotNetLogo,
                                Parameter = AboutThisApp.SpbLink
                            },
                        new LocalizableMenuItem
                            {
                                ResourceName = "Moscow",
                                Command = this.LaunchBrowserCommand,
                                ImageSource = LogoService.MskDotNetLogo,
                                Parameter = AboutThisApp.MoscowLink
                            },
                        new LocalizableMenuItem
                            {
                                ResourceName = "Saratov",
                                Command = this.LaunchBrowserCommand,
                                ImageSource = LogoService.SarDotNetLogo,
                                Parameter = AboutThisApp.SaratovLink
                            },
                        new LocalizableMenuItem
                            {
                                ResourceName = "Krasnoyarsk",
                                Command = this.LaunchBrowserCommand,
                                ImageSource = LogoService.KryDotNetLogo,
                                Parameter = AboutThisApp.KrasnoyarskLink
                            }
                    });
        }

        public IList<Language> Languages => EnumExtension.GetEnumValues<Language>().ToList();

        public Language SelectedLanguage
        {
            get => this.selectedLanguage;
            set
            {
                if (this.SetProperty(ref this.selectedLanguage, value))
                {
                    XamarinEvolve.Clients.Portable.Helpers.Settings.CurrentLanguage = this.selectedLanguage;
                    MessagingCenter.Send<object, CultureChangedMessage>(
                        this,
                        string.Empty,
                        new CultureChangedMessage(this.selectedLanguage.GetLanguageCode()));
                }
            }
        }

        public CustomObservableCollection<LocalizableMenuItem> AboutItems { get; } = new CustomObservableCollection<LocalizableMenuItem>();

        public CustomObservableCollection<LocalizableMenuItem> Communities { get; } = new CustomObservableCollection<LocalizableMenuItem>();

        public string Copyright => AboutThisApp.Copyright;

        public bool AppToWebLinkingEnabled => FeatureFlags.AppToWebLinkingEnabled;

        public ImageSource BackgroundImage { get; set; }

        public string AppVersion =>
            $"{this.Resources["Version"]} {DependencyService.Get<IAppVersionProvider>()?.AppVersion ?? "1.0"}";

        private void NotifyViewModel()
        {
            var cultureInfo = new CultureInfo(this.SelectedLanguage.GetLanguageCode());
            DependencyService.Get<ILocalize>().SetLocale(cultureInfo); // set the Thread for locale-aware methods
            AppResources.Culture = cultureInfo;

            this.OnPropertyChanged(nameof(this.AppVersion));

            this.AboutItems.ForEach(x => x.Update());
            this.Communities.ForEach(x => x.Update());
        }
    }
}
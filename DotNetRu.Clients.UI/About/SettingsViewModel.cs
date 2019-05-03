namespace DotNetRu.Clients.Portable.ViewModel
{
    using System.Collections.Generic;
    using System.Linq;

    using DotNetRu.Clients.Portable.Helpers;
    using DotNetRu.Clients.Portable.Interfaces;
    using DotNetRu.Clients.Portable.Services;
    using DotNetRu.Clients.UI.Localization;
    using DotNetRu.Utils.Helpers;
    using Xamarin.Essentials;
    using Xamarin.Forms;

    public class SettingsViewModel : ViewModelBase
    {
        private readonly IToast toaster;

        private Language selectedLanguage;

        public SettingsViewModel(Command openCreditsCommand, Command openTechnologiesUsedCommand, Command openFriendsCommand)
        {
            this.toaster = DependencyService.Get<IToast>();

            this.OpenCreditsCommand = openCreditsCommand;
            this.CopyAppVersionCommand = new Command(async () =>
            {
                await Clipboard.SetTextAsync(AppVersion);                
                toaster.SendToast(AppResources.ResourceManager.GetString("CopiedToClipboard"));
            });
            this.OpenTechnologiesUsedCommand = openTechnologiesUsedCommand;
            this.OpenFriendsCommand = openFriendsCommand;

            MessagingCenter.Subscribe<LocalizedResources>(
                this,
                MessageKeys.LanguageChanged,
                sender => this.UpdateViewModel());

            this.selectedLanguage = LanguageService.GetCurrentLanguage();
        }

        public Command OpenCreditsCommand { get; set; }

        public Command CopyAppVersionCommand { get; set; }

        public Command OpenTechnologiesUsedCommand { get; set; }

        public Command OpenFriendsCommand { get; set; }

        public IList<Language> Languages => EnumExtension.GetEnumValues<Language>().ToList();

        public Language SelectedLanguage
        {
            get => this.selectedLanguage;
            set
            {
                if (this.SetProperty(ref this.selectedLanguage, value))
                {
                    Helpers.Settings.CurrentLanguage = this.selectedLanguage;
                    MessagingCenter.Send<object, CultureChangedMessage>(
                        this,
                        string.Empty,
                        new CultureChangedMessage(this.selectedLanguage.GetLanguageCode()));
                }
            }
        }

        public string Copyright => AboutThisApp.Copyright;

        public bool AppToWebLinkingEnabled => FeatureFlags.AppToWebLinkingEnabled;

        public ImageSource BackgroundImage { get; set; }

        public string AppVersion =>
            $"{AppResources.Version} {DependencyService.Get<IAppVersionProvider>()?.AppVersion ?? "1.0"}";

        private void UpdateViewModel()
        {
            this.OnPropertyChanged(nameof(this.AppVersion));
        }
    }
}

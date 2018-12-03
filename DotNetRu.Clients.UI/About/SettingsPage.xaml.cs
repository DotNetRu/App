namespace DotNetRu.Clients.UI.Pages.About
{
    using System;

    using DotNetRu.Clients.Portable.Model;
    using DotNetRu.Clients.Portable.ViewModel;
    using DotNetRu.Clients.UI.Helpers;
    using DotNetRu.Clients.UI.Localization;
    using DotNetRu.Clients.UI.Pages.Friends;
    using DotNetRu.Clients.UI.Pages.Info;
    using DotNetRu.Utils.Helpers;
    using Xamarin.Essentials;
    using Xamarin.Forms;

    public partial class SettingsPage
    {
        public SettingsPage()
        {            
            this.InitializeComponent();

            var openTechnologiesUsedCommand = new Command(
                async () => await NavigationService.PushAsync(this.Navigation, new TechnologiesUsedPage()));
            var openCreditsCommand = new Command(
                async () => await this.DisplayAlert(
                                "Credits",
                                AppResources.Credits,
                                "OK"));
            var openFriendsCommand = new Command(
                async () => await NavigationService.PushAsync(this.Navigation, new FriendsPage()));

            var settingsViewModel = new SettingsViewModel(openCreditsCommand, openTechnologiesUsedCommand, openFriendsCommand);

            this.BindingContext = settingsViewModel;
        }

        public override AppPage PageType => AppPage.Information;

        public Command OpenCreditsCommand => new Command(
            async () => await this.DisplayAlert(
                            "Credits",
                            AppResources.Credits,
                            "OK"));

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName == "Title")
            {
                MessagingCenter.Send(this, MessageKeys.UpdateTitles);
            }
        }

        private async void Friends_OnClicked(object sender, EventArgs e)
        {
            await NavigationService.PushAsync(this.Navigation, new FriendsPage());
        }

        private async void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
        {
            await Browser.OpenAsync(AboutThisApp.DotNetRuLink, BrowserLaunchMode.SystemPreferred);
        }
    }
}

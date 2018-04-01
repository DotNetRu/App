namespace DotNetRu.Clients.UI.Pages.About
{
    using System;

    using DotNetRu.Clients.Portable.ApplicationResources;
    using DotNetRu.Clients.Portable.Model;
    using DotNetRu.Clients.Portable.ViewModel;
    using DotNetRu.Clients.UI.Helpers;
    using DotNetRu.Clients.UI.Pages.Friends;
    using DotNetRu.Clients.UI.Pages.Info;
    using DotNetRu.Utils.Helpers;

    using Plugin.Share;
    using Plugin.Share.Abstractions;

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
                                AppResources.ResourceManager.GetString("Credits", AppResources.Culture),
                                "OK"));

            var settingsViewModel = new SettingsViewModel(openTechnologiesUsedCommand, openCreditsCommand);

            this.BindingContext = settingsViewModel;

            var adjust = Device.RuntimePlatform != Device.Android ? 1 : -settingsViewModel.AboutItems.Count + 1;
            this.ListViewAbout.HeightRequest = (settingsViewModel.AboutItems.Count * this.ListViewAbout.RowHeight) - adjust;
            this.ListViewAbout.ItemTapped += (sender, e) => this.ListViewAbout.SelectedItem = null;

            adjust = Device.RuntimePlatform != Device.Android ? 1 : -settingsViewModel.Communities.Count + 1;
            this.ListViewCommunities.HeightRequest =
                (settingsViewModel.Communities.Count * this.ListViewCommunities.RowHeight) - adjust;
            this.ListViewCommunities.ItemTapped += (sender, e) => this.ListViewCommunities.SelectedItem = null;
        }

        public override AppPage PageType => AppPage.Information;

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
            var primaryColor = (Color)Application.Current.Resources["Primary"];

            await CrossShare.Current.OpenBrowser(
                AboutThisApp.DotNetRuLink,
                new BrowserOptions
                    {
                        ChromeShowTitle = true,
                        ChromeToolbarColor =
                            new ShareColor
                                {
                                    A = 255,
                                    R = Convert.ToInt32(primaryColor.R),
                                    G = Convert.ToInt32(primaryColor.G),
                                    B = Convert.ToInt32(primaryColor.B)
                                },
                        UseSafariReaderMode = true,
                        UseSafariWebViewController = true
                    });
        }
    }
}
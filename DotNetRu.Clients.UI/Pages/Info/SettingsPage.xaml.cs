namespace XamarinEvolve.Clients.UI
{
    using System;

    using Xamarin.Forms;

    using XamarinEvolve.Clients.Portable;
    using XamarinEvolve.Clients.UI.Pages.Info;
    using XamarinEvolve.Utils.Helpers;

    public partial class SettingsPage
    {

        private readonly SettingsViewModel settingsViewModel = new SettingsViewModel();

        public SettingsPage()
        {
            this.InitializeComponent();

            this.BindingContext = this.settingsViewModel;
            var adjust = Device.RuntimePlatform != Device.Android ? 1 : -this.settingsViewModel.AboutItems.Count + 1;
            this.ListViewAbout.HeightRequest = (this.settingsViewModel.AboutItems.Count * this.ListViewAbout.RowHeight) - adjust;
            this.ListViewAbout.ItemTapped += (sender, e) => this.ListViewAbout.SelectedItem = null;

            adjust = Device.RuntimePlatform != Device.Android ? 1 : -this.settingsViewModel.Communities.Count + 1;
            this.ListViewCommunities.HeightRequest =
                (this.settingsViewModel.Communities.Count * this.ListViewCommunities.RowHeight) - adjust;
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

        private async void Technologies_OnClicked(object sender, EventArgs e)
        {
            await NavigationService.PushAsync(this.Navigation, new TechnologiesUsed());
        }
    }
}
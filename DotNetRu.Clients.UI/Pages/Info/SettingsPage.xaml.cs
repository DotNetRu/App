namespace DotNetRu.Clients.UI.Pages.Info
{
    using System;

    using DotNetRu.Clients.Portable.Model;
    using DotNetRu.Clients.Portable.ViewModel;
    using DotNetRu.Clients.UI.Helpers;
    using DotNetRu.Clients.UI.Pages.Friends;
    using DotNetRu.Utils.Helpers;

    using Xamarin.Forms;

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
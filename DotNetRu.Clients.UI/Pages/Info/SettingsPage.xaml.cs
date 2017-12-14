using System;

using Xamarin.Forms;

using XamarinEvolve.Clients.Portable;
using XamarinEvolve.Clients.UI.Pages.Info;

namespace XamarinEvolve.Clients.UI
{
    using XamarinEvolve.Utils.Helpers;

	public partial class SettingsPage
	{
    public override AppPage PageType => AppPage.Information;

        readonly SettingsViewModel vm;

        public SettingsPage()
        {
            this.InitializeComponent();

            this.BindingContext = this.vm = new SettingsViewModel();
            var adjust = Device.RuntimePlatform != Device.Android ? 1 : -this.vm.AboutItems.Count + 1;
            this.ListViewAbout.HeightRequest = (this.vm.AboutItems.Count * this.ListViewAbout.RowHeight) - adjust;
            this.ListViewAbout.ItemTapped += (sender, e) => this.ListViewAbout.SelectedItem = null;

            adjust = Device.RuntimePlatform != Device.Android ? 1 : -this.vm.Communities.Count + 1;
            this.ListViewCommunities.HeightRequest = (this.vm.Communities.Count * this.ListViewCommunities.RowHeight) - adjust;
            this.ListViewCommunities.ItemTapped += (sender, e) => this.ListViewCommunities.SelectedItem = null;
        }

        bool dialogShown;
        int count;

   

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName == "Title")
                MessagingCenter.Send(this, MessageKeys.UpdateTitles);
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


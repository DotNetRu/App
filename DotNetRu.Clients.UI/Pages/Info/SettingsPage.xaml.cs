using System;

using Xamarin.Forms;

using XamarinEvolve.Clients.Portable;

namespace XamarinEvolve.Clients.UI
{
	using XamarinEvolve.Utils.Helpers;

	public partial class SettingsPage : BasePage
  {
    public override AppPage PageType => AppPage.Information;

    SettingsViewModel vm;

    public SettingsPage()
    {
        this.InitializeComponent();

        this.BindingContext = this.vm = new SettingsViewModel();
      var adjust = Device.RuntimePlatform != Device.Android ? 1 : -this.vm.AboutItems.Count + 1;
        this.ListViewAbout.HeightRequest = (this.vm.AboutItems.Count * this.ListViewAbout.RowHeight) - adjust;
        this.ListViewAbout.ItemTapped += (sender, e) => this.ListViewAbout.SelectedItem = null;
      adjust = Device.RuntimePlatform != Device.Android ? 1 : -this.vm.TechnologyItems.Count + 1;
        this.ListViewTechnology.HeightRequest = (this.vm.TechnologyItems.Count * this.ListViewTechnology.RowHeight) - adjust;
        this.ListViewTechnology.ItemTapped += (sender, e) => this.ListViewTechnology.SelectedItem = null;
    }

    bool dialogShown;
    int count;

    async void OnTapGestureRecognizerTapped(object sender, EventArgs args)
    {
        this.count++;
      if (this.dialogShown || this.count < 8)
        return;

        this.dialogShown = true;

      App.Logger.Track("AppCreditsFound-8MoreThan92");

      await this.DisplayAlert("Credits",
        AboutThisApp.Credits, "OK");
    }
  }
}


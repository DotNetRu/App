using System;
using Xamarin.Forms;
using XamarinEvolve.Clients.Portable;
using FormsToolkit;
using XamarinEvolve.Utils;

namespace XamarinEvolve.Clients.UI
{
  public partial class SettingsPage : BasePage
  {
    public override AppPage PageType => AppPage.Settings;

    SettingsViewModel vm;

    public SettingsPage()
    {
      InitializeComponent();


      BindingContext = vm = new SettingsViewModel();
      var adjust = Device.OS != TargetPlatform.Android ? 1 : -vm.AboutItems.Count + 1;
      ListViewAbout.HeightRequest = (vm.AboutItems.Count * ListViewAbout.RowHeight) - adjust;
      ListViewAbout.ItemTapped += (sender, e) => ListViewAbout.SelectedItem = null;
      adjust = Device.OS != TargetPlatform.Android ? 1 : -vm.TechnologyItems.Count + 1;
      ListViewTechnology.HeightRequest = (vm.TechnologyItems.Count * ListViewTechnology.RowHeight) - adjust;
      ListViewTechnology.ItemTapped += (sender, e) => ListViewTechnology.SelectedItem = null;
    }

    bool dialogShown;
    int count;

    async void OnTapGestureRecognizerTapped(object sender, EventArgs args)
    {
      count++;
      if (dialogShown || count < 8)
        return;

      dialogShown = true;

      App.Logger.Track("AppCreditsFound-8MoreThan92");

      await DisplayAlert("Credits",
        AboutThisApp.Credits, "OK");
    }

    protected override void OnDisappearing()
    {
      base.OnDisappearing();
      MessagingService.Current.Unsubscribe(MessageKeys.NavigateToSyncMobileToWebViewModel);
      MessagingService.Current.Unsubscribe(MessageKeys.NavigateToSyncWebToMobileViewModel);
    }
  }
}


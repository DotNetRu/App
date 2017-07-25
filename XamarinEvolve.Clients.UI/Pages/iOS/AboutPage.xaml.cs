﻿using Xamarin.Forms;
using XamarinEvolve.Clients.Portable;

namespace XamarinEvolve.Clients.UI
{
  public partial class AboutPage : BasePage
  {
    public override AppPage PageType => AppPage.Information;

    AboutViewModel vm;
    IPushNotifications push;

    public AboutPage()
    {
      InitializeComponent();
      BindingContext = vm = new AboutViewModel();
      push = DependencyService.Get<IPushNotifications>();
      var adjust = Device.OS != TargetPlatform.Android ? 1 : -vm.AboutItems.Count + 1;
      ListViewAbout.HeightRequest = (vm.AboutItems.Count * ListViewAbout.RowHeight) - adjust;
      ListViewAbout.ItemTapped += (sender, e) => ListViewAbout.SelectedItem = null;
      ListViewInfo.HeightRequest = (vm.InfoItems.Count * ListViewInfo.RowHeight) - adjust;

      ListViewAccount.HeightRequest = (vm.AccountItems.Count * ListViewAccount.RowHeight) - adjust;

      ListViewAbout.ItemSelected += async (sender, e) =>
      {
        if (ListViewAbout.SelectedItem == null)
          return;

        await NavigationService.PushAsync(Navigation, new SettingsPage());

        ListViewAbout.SelectedItem = null;
      };

      ListViewInfo.ItemSelected += async (sender, e) =>
      {
        var item = ListViewInfo.SelectedItem as XamarinEvolve.Clients.Portable.MenuItem;
        if (item == null)
          return;
        Page page = null;
        switch (item.Parameter)
        {
          case "sponsors":
            page = new SponsorsPage();
            break;
        }

        if (page == null)
          return;
        if (Device.OS == TargetPlatform.iOS)
          await NavigationService.PushAsync(((Page) this.Parent.Parent).Navigation, page);
        else
          await NavigationService.PushAsync(Navigation, page);

        ListViewInfo.SelectedItem = null;
      };
      isRegistered = push.IsRegistered;
    }

    bool isRegistered;

    protected override void OnAppearing()
    {
      base.OnAppearing();

      if (!isRegistered && Settings.Current.AttemptedPush)
      {
        push.RegisterForNotifications();
      }
      isRegistered = push.IsRegistered;
      vm.UpdateItems();
    }

    public void OnResume()
    {
      OnAppearing();
    }
  }
}


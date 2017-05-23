﻿using System;
using System.Collections.Generic;

using Xamarin.Forms;
using XamarinEvolve.Clients.Portable;
using FormsToolkit;

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
			ListViewAccount.ItemSelected += async (sender, e) =>
			{
				var item = ListViewAccount.SelectedItem as XamarinEvolve.Clients.Portable.MenuItem;
				if (item == null)
					return;
				Page page = null;
				switch (item.Parameter)
				{
					case "mobiletowebsync":
						page = new SyncMobileToWebPage();
						break;
					case "webtomobilesync":
						page = new SyncWebToMobilePage();
						break;
				}
				ListViewAccount.SelectedItem = null;

				if (page == null)
					return;
				
				await NavigationService.PushAsync(Navigation, page);
			};

            ListViewAbout.ItemSelected += async (sender, e) => 
                {
                    if(ListViewAbout.SelectedItem == null)
                        return;

                    await NavigationService.PushAsync(Navigation, new SettingsPage());

                    ListViewAbout.SelectedItem = null;
                };

            ListViewInfo.ItemSelected += async (sender, e) => 
                {
                    var item = ListViewInfo.SelectedItem as XamarinEvolve.Clients.Portable.MenuItem;
                    if(item == null)
                        return;
                    Page page = null;
                    switch(item.Parameter)
                    {
                        case "evaluations":
                            page = new EvaluationsPage ();
                            break;
                        case "venue":
                            page = new VenuePage();
                            break;
                        case "code-of-conduct":
                            page = new CodeOfConductPage();
                            break;
                        case "wi-fi":
                            page = new WiFiInformationPage();
                            break;
                        case "sponsors":
                            page = new SponsorsPage();
                            break;
                        case "floor-maps":
                            App.Logger.TrackPage(AppPage.FloorMap.ToString());
                            page = new FloorMapsCarouselPage();
                            break;
					}

                    if(page == null)
                        return;
                    if(Device.OS == TargetPlatform.iOS && page is VenuePage)
                        await NavigationService.PushAsync(((Page)this.Parent.Parent).Navigation, page);
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


﻿using System;
using Xamarin.Forms;
using XamarinEvolve.DataObjects;
using FormsToolkit;
using XamarinEvolve.Clients.Portable;

namespace XamarinEvolve.Clients.UI
{
	public partial class SponsorsPage : BasePage
	{
		public override AppPage PageType => AppPage.Sponsors;

        SponsorsViewModel vm;
        SponsorsViewModel ViewModel => vm ?? (vm = BindingContext as SponsorsViewModel); 

        public SponsorsPage()
        {
            InitializeComponent();
            BindingContext = new SponsorsViewModel(Navigation);

            if (Device.OS == TargetPlatform.Android)
                ListViewSponsors.Effects.Add (Effect.Resolve ("Xpirit.ListViewSelectionOnTopEffect"));

            if (Device.OS == TargetPlatform.Windows || Device.OS == TargetPlatform.WinPhone)
            {
                ToolbarItems.Add(new ToolbarItem
                {
                    Text = "Refresh",
                    Icon = "toolbar_refresh.png",
                    Command = ViewModel.ForceRefreshCommand
                });
            }
            ListViewSponsors.ItemSelected += async (sender, e) => 
            {
                    var sponsor = ListViewSponsors.SelectedItem as Sponsor;
                    if(sponsor == null)
                        return;
                    var sponsorDetails = new SponsorDetailsPage();

                    sponsorDetails.Sponsor = sponsor;
                    await NavigationService.PushAsync(Navigation, sponsorDetails);
                    ListViewSponsors.SelectedItem = null;
            };
        }

        void ListViewTapped (object sender, ItemTappedEventArgs e)
        {
            var list = sender as ListView;
            if (list == null)
                return;
            list.SelectedItem = null;
        }
            
        protected override void OnAppearing()
        {
            base.OnAppearing();

            ListViewSponsors.ItemTapped += ListViewTapped;
            if (ViewModel.SponsorsGrouped.Count == 0)
                ViewModel.LoadSponsorsCommand.Execute(false);

        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            ListViewSponsors.ItemTapped -= ListViewTapped;
        }
    }
}
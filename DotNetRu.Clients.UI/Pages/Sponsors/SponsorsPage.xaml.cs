namespace XamarinEvolve.Clients.UI
{
    using DotNetRu.DataStore.Audit.Models;

    using Xamarin.Forms;

    using XamarinEvolve.Clients.Portable;

    public partial class SponsorsPage
    {
		public override AppPage PageType => AppPage.Sponsors;

        SponsorsViewModel vm;
        SponsorsViewModel ViewModel => this.vm ?? (this.vm = this.BindingContext as SponsorsViewModel); 

        public SponsorsPage()
        {
            this.InitializeComponent();
            this.BindingContext = new SponsorsViewModel(this.Navigation);

            if (Device.RuntimePlatform == Device.Android) this.ListViewSponsors.Effects.Add (Effect.Resolve ("Xpirit.ListViewSelectionOnTopEffect"));

            if (Device.RuntimePlatform == Device.UWP)
            {
                this.ToolbarItems.Add(new ToolbarItem
                {
                    Text = "Refresh",
                    Icon = "toolbar_refresh.png",
                    Command = this.ViewModel.ForceRefreshCommand
                });
            }

            this.ListViewSponsors.ItemSelected += async (sender, e) => 
            {
                if(!(this.ListViewSponsors.SelectedItem is Sponsor sponsor))
                        return;
                    var sponsorDetails = new SponsorDetailsPage();

                    sponsorDetails.Sponsor = sponsor;
                    await NavigationService.PushAsync(this.Navigation, sponsorDetails);
                this.ListViewSponsors.SelectedItem = null;
            };
        }

        void ListViewTapped (object sender, ItemTappedEventArgs e)
        {
            if (!(sender is ListView list))
                return;
            list.SelectedItem = null;
        }
            
        protected override void OnAppearing()
        {
            base.OnAppearing();

            this.ListViewSponsors.ItemTapped += this.ListViewTapped;
            if (this.ViewModel.SponsorsGrouped.Count == 0)
            {
                this.ViewModel.LoadSponsorsCommand.Execute(false);
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            this.ListViewSponsors.ItemTapped -= this.ListViewTapped;
        }
    }
}
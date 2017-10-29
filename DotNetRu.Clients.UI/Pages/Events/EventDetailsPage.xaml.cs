using Xamarin.Forms;
using XamarinEvolve.Clients.Portable;

namespace XamarinEvolve.Clients.UI
{
    using DotNetRu.DataStore.Audit.Models;

    public partial class EventDetailsPage : BasePage
	{
		public override AppPage PageType => AppPage.Event;

        EventDetailsViewModel ViewModel => this.vm ?? (this.vm = this.BindingContext as EventDetailsViewModel);
        EventDetailsViewModel vm;

        public EventDetailsPage()
        {
            this.InitializeComponent();

            this.ListViewSponsors.ItemSelected += async (sender, e) => 
                {
                    if(!(this.ListViewSponsors.SelectedItem is FriendModel sponsor))
                        return;

                    var sponsorDetails = new FriendDetailsPage
                        {
                            FriendModel = sponsor
                        };

                    await NavigationService.PushAsync(this.Navigation, sponsorDetails);

                    this.ListViewSponsors.SelectedItem = null;
                };
        }

        public FeaturedEvent Event
        {
            get => this.ViewModel.Event;
            set => this.BindingContext = new EventDetailsViewModel(this.Navigation, value);
        }

		protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            this.vm = null;

            var adjust = Device.RuntimePlatform != Device.Android ? 1 : -this.ViewModel.Sponsors.Count + 1;
            this.ListViewSponsors.HeightRequest = this.ListViewSponsors.RowHeight - adjust;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            this.ViewModel.LoadEventDetailsCommand.Execute(null);

        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }
    }
}


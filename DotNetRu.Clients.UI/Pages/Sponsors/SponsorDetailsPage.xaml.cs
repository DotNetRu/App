using Xamarin.Forms;
using XamarinEvolve.Clients.Portable;

namespace XamarinEvolve.Clients.UI
{
    using DotNetRu.DataStore.Audit.Models;

    public partial class SponsorDetailsPage : BasePage
	{
		public override AppPage PageType => AppPage.Sponsor;

        SponsorDetailsViewModel ViewModel => this.vm ?? (this.vm = this.BindingContext as SponsorDetailsViewModel);
        SponsorDetailsViewModel vm;

        public SponsorDetailsPage()
        {
            this.InitializeComponent();           
        }


        public Sponsor Sponsor
        {
            get => this.ViewModel.Sponsor;
            set 
			{
			    this.BindingContext = new SponsorDetailsViewModel(this.Navigation, value);
			    this.ItemId = value?.Name;
			}
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            this.vm = null;
            var adjust = Device.RuntimePlatform != Device.Android ? 1 : -this.ViewModel.FollowItems.Count + 1;
            this.ListViewFollow.HeightRequest = (this.ViewModel.FollowItems.Count * this.ListViewFollow.RowHeight) - adjust;
        }
    }
}


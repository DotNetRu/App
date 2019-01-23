namespace DotNetRu.Clients.UI.Pages.Sessions
{
    using System.Linq;

    using DotNetRu.Clients.Portable.Model;
    using DotNetRu.Clients.Portable.ViewModel;
    using DotNetRu.Clients.UI.Helpers;
    using DotNetRu.DataStore.Audit.Models;

    using Xamarin.Forms;

    public partial class MeetupDetailsPage
    {
        private MeetupViewModel meetupViewModel;

        public MeetupDetailsPage(MeetupModel meetup)
        {
            this.InitializeComponent();

            this.BindingContext = this.meetupViewModel = new MeetupViewModel(this.Navigation, meetup);

            this.ItemId = meetup.Id;

            this.ListViewTalks.ItemSelected += async (sender, e) =>
                {
                    if (!(this.ListViewTalks.SelectedItem is SessionModel session))
                    {
                        return;
                    }

                    var sessionDetails = new TalkPage(session.Talk);

                    await NavigationService.PushAsync(this.Navigation, sessionDetails);
                    this.ListViewTalks.SelectedItem = null;
                };
        }

        public override AppPage PageType => AppPage.Meetup;

        private MeetupViewModel MeetupViewModel =>
            this.meetupViewModel ?? (this.meetupViewModel = this.BindingContext as MeetupViewModel);

        protected override void OnAppearing()
        {
            base.OnAppearing();

            this.ListViewTalks.ItemTapped += this.ListViewTapped;

            var count = this.MeetupViewModel?.Sessions?.Count() ?? 0;
            var adjust = Device.RuntimePlatform != Device.Android ? 1 : -count + 1;
            if ((this.MeetupViewModel?.Sessions?.Count() ?? 0) > 0)
            {
                this.ListViewTalks.HeightRequest = (count * this.ListViewTalks.RowHeight) - adjust;
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            this.ListViewTalks.ItemTapped -= this.ListViewTapped;
        }

        private void ListViewTapped(object sender, ItemTappedEventArgs e)
        {
            if (!(sender is ListView list))
            {
                return;
            }

            list.SelectedItem = null;
        }
    }
}

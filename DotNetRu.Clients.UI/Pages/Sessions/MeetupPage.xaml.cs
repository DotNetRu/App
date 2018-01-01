using DotNetRu.Clients.Portable.Model;
using DotNetRu.Clients.Portable.ViewModel;

namespace XamarinEvolve.Clients.UI
{
    using System.Linq;

    using DotNetRu.DataStore.Audit.Models;
    using DotNetRu.DataStore.Audit.Services;

    using Xamarin.Forms;

    using XamarinEvolve.Clients.Portable;

    public partial class MeetupPage
    {

        private MeetupViewModel meetupViewModel;

        /// <summary>
        /// Design-time only
        /// </summary>
        public MeetupPage()
        {
            this.InitializeComponent();

            var meetupModel = MeetupService.Meetups.First();
                
            this.BindingContext = this.meetupViewModel = new MeetupViewModel(this.Navigation, meetupModel);

            this.UpdatePage();

            // this.ToolbarItems.Add(this.filterItem);
            this.ListViewTalks.ItemSelected += async (sender, e) =>
                {
                    if (!(this.ListViewTalks.SelectedItem is TalkModel session))
                    {
                        return;
                    }

                    var sessionDetails = new TalkPage(session);

                    await NavigationService.PushAsync(this.Navigation, sessionDetails);
                    this.ListViewTalks.SelectedItem = null;
                };
        }

        public MeetupPage(MeetupModel meetup = null)
        {
            this.InitializeComponent();

            var venue = VenueService.Venues.Single(x => x.Id == meetup.VenueID);

            this.BindingContext = this.meetupViewModel = new MeetupViewModel(this.Navigation, meetup, venue);

            // this.filterItem = new ToolbarItem { Text = "Filter" };

            // if (Device.RuntimePlatform != Device.iOS)
            // {
            // this.filterItem.Icon = "toolbar_filter.png";
            // }

            // this.filterItem.Command = new Command(
            // async () =>
            // {
            // if (this.meetupViewModel.IsBusy) return;
            // await NavigationService.PushModalAsync(
            // this.Navigation,
            // new EvolveNavigationPage(new FilterSessionsPage()));
            // });

            // this.ToolbarItems.Add(this.filterItem);
            this.ListViewTalks.ItemSelected += async (sender, e) =>
                {
                    if (!(this.ListViewTalks.SelectedItem is TalkModel session))
                    {
                        return;
                    }

                    var sessionDetails = new TalkPage(session);

                    await NavigationService.PushAsync(this.Navigation, sessionDetails);
                    this.ListViewTalks.SelectedItem = null;
                };
        }

        public override AppPage PageType => AppPage.Meetup;

        private MeetupViewModel MeetupViewModel =>
            this.meetupViewModel ?? (this.meetupViewModel = this.BindingContext as MeetupViewModel);

        private void ListViewTapped(object sender, ItemTappedEventArgs e)
        {
            if (!(sender is ListView list))
            {
                return;
            }

            list.SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            this.ListViewTalks.ItemTapped += this.ListViewTapped;

            this.UpdatePage();

            var count = this.MeetupViewModel?.Talks?.Count ?? 0;
            var adjust = Device.RuntimePlatform != Device.Android ? 1 : -count + 1;
            if ((this.MeetupViewModel?.Talks?.Count ?? 0) > 0)
            {
                this.ListViewTalks.HeightRequest = (count * this.ListViewTalks.RowHeight) - adjust;
            }
        }

        private void UpdatePage()
        {
            // Load if none
            if ((this.MeetupViewModel?.Talks?.Count ?? 0) == 0)
            {
                this.MeetupViewModel?.LoadTalksCommand?.Execute(null);
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            this.ListViewTalks.ItemTapped -= this.ListViewTapped;
        }

        public void OnResume()
        {
            this.UpdatePage();
        }
    }
}
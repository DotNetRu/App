using DotNetRu.Clients.Portable.Model;
using DotNetRu.Clients.UI.Meetups;
using DotNetRu.DataStore.Audit.Models;
using System.Collections.Generic;
using Xamarin.Forms;

namespace DotNetRu.Clients.UI.Pages.Sessions
{
    public partial class MeetupDetailsPage
    {
        private MeetupDetailsViewModel meetupViewModel;

        public MeetupDetailsPage(MeetupModel meetup)
        {
            this.InitializeComponent();

            this.BindingContext = this.meetupViewModel = new MeetupDetailsViewModel(this.Navigation, meetup);

            this.ItemId = meetup.Id;
        }

        public override AppPage PageType => AppPage.Meetup;

        private MeetupDetailsViewModel MeetupViewModel => meetupViewModel ?? (meetupViewModel = BindingContext as MeetupDetailsViewModel);

        protected override void OnAppearing()
        {
            base.OnAppearing();

            AdjustListView(ListViewTalks, MeetupViewModel?.Sessions);
            AdjustListView(ListViewFriends, MeetupViewModel?.Friends);
        }

        private void AdjustListView<T>(ListView listView, IReadOnlyCollection<T> items)
        {
            var count = items?.Count ?? 0;
            if (count == 0)
            {
                return;
            }

            int adjust;
            if (Device.RuntimePlatform == Device.Android)
            {
                adjust = 1 - count;
            }
            else
            {
                adjust = 1;
            }

            listView.HeightRequest = count * listView.RowHeight - adjust;
        }
    }
}

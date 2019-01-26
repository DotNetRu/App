using System.Collections.Generic;

using DotNetRu.Clients.Portable.Model;
using DotNetRu.Clients.Portable.ViewModel;
using DotNetRu.Clients.UI.Controls;
using DotNetRu.Clients.UI.Helpers;
using DotNetRu.Clients.UI.Pages.Friends;
using DotNetRu.DataStore.Audit.Models;

using Xamarin.Forms;

namespace DotNetRu.Clients.UI.Pages.Sessions
{
    public partial class MeetupDetailsPage
    {
        private MeetupViewModel meetupViewModel;

        public MeetupDetailsPage(MeetupModel meetup)
        {
            InitializeComponent();

            BindingContext = meetupViewModel = new MeetupViewModel(Navigation, meetup);

            ItemId = meetup.Id;

            ListViewTalks.ItemSelected += OnSessionTapped;

            ListViewFriends.ItemSelected += OnFriendTapped;
        }        

        private async void OnSessionTapped(object sender, SelectedItemChangedEventArgs e)
        {
            if (!(ListViewTalks.SelectedItem is SessionModel session))
            {
                return;
            }

            var sessionDetails = new TalkPage(session.Talk);

            await NavigationService.PushAsync(Navigation, sessionDetails);
            ListViewTalks.SelectedItem = null;
        }

        private async void OnFriendTapped(object sender, SelectedItemChangedEventArgs e)
        {
            if (!(ListViewFriends.SelectedItem is FriendModel sponsor))
            {
                return;
            }

            var sponsorDetails = new FriendDetailsPage
            {
                FriendModel = sponsor
            };

            await NavigationService.PushAsync(Navigation, sponsorDetails);
            ListViewFriends.SelectedItem = null;
        }

        public override AppPage PageType => AppPage.Meetup;

        private MeetupViewModel MeetupViewModel => meetupViewModel ?? (meetupViewModel = BindingContext as MeetupViewModel);

        protected override void OnAppearing()
        {
            base.OnAppearing();

            ListViewTalks.ItemTapped += ListViewTapped;
            ListViewFriends.ItemTapped += ListViewTapped;

            AdjustListView(ListViewTalks, MeetupViewModel?.Sessions);
            AdjustListView(ListViewFriends, MeetupViewModel?.Friends);            
        }

        private void AdjustListView<T>(NonScrollableListView listView, IReadOnlyCollection<T> items)
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

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            ListViewFriends.ItemTapped -= ListViewTapped;
            ListViewTalks.ItemTapped -= ListViewTapped;
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

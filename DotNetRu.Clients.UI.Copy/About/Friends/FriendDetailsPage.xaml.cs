namespace DotNetRu.Clients.UI.Pages.Friends
{
    using System.Linq;

    using DotNetRu.Clients.Portable.Model;
    using DotNetRu.Clients.Portable.ViewModel;
    using DotNetRu.Clients.UI.Helpers;
    using DotNetRu.Clients.UI.Pages.Sessions;
    using DotNetRu.DataStore.Audit.Models;

    using Microsoft.Maui;

    public partial class FriendDetailsPage
    {
        private FriendDetailsViewModel friendDetailsViewModel;

        public FriendDetailsPage()
        {
            this.InitializeComponent();

            this.ListViewMeetups.ItemTapped += (sender, e) => this.ListViewMeetups.SelectedItem = null;
            this.ListViewMeetups.ItemSelected += async (sender, e) =>
            {
                if (!(this.ListViewMeetups.SelectedItem is MeetupModel ev))
                {
                    return;
                }

                var eventSessions = new MeetupDetailsPage(ev);
                await NavigationService.PushAsync(this.Navigation, eventSessions);
                this.ListViewMeetups.SelectedItem = null;
            };            
        }

        public override AppPage PageType => AppPage.Friend;

        public FriendModel FriendModel
        {
            get => this.FriendDetailsViewModel.FriendModel;
            set
            {
                this.BindingContext = new FriendDetailsViewModel(this.Navigation, value);
                this.ItemId = value?.Name;
            }
        }

        private FriendDetailsViewModel FriendDetailsViewModel => this.friendDetailsViewModel ?? (this.friendDetailsViewModel = this.BindingContext as FriendDetailsViewModel);

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            this.friendDetailsViewModel = null;
            var adjust = Device.RuntimePlatform != Device.Android ? 1 : -this.FriendDetailsViewModel.FollowItems.Count + 1;
            this.ListViewFollow.HeightRequest =
                (this.FriendDetailsViewModel.FollowItems.Count * this.ListViewFollow.RowHeight) - adjust;

            adjust = Device.RuntimePlatform != Device.Android ? 1 : -this.FriendDetailsViewModel.FriendModel.Meetups.Count() + 1;
            this.ListViewMeetups.HeightRequest =
                (this.FriendDetailsViewModel.FriendModel.Meetups.Count() * this.ListViewMeetups.RowHeight) - adjust;
        }
    }
}

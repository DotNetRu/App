using System.Linq;
using DotNetRu.Clients.Portable.Model;
using DotNetRu.Clients.Portable.ViewModel;
using DotNetRu.Clients.UI.Helpers;
using DotNetRu.DataStore.Audit.Models;
using Xamarin.Forms;

namespace DotNetRu.Clients.UI.Pages.Friends
{
    public partial class FriendsPage
    {
        private FriendsViewModel friendsViewModel;

        public FriendsPage()
        {
            this.InitializeComponent();
            this.BindingContext = new FriendsViewModel(this.Navigation);

            if (Device.RuntimePlatform == Device.UWP)
            {
                this.ToolbarItems.Add(new ToolbarItem { Text = "Refresh", Icon = "toolbar_refresh.png" });
            }

            this.ListViewFriends.ItemSelected += async (sender, e) =>
                {
                    if (!(this.ListViewFriends.SelectedItem is FriendModel sponsor))
                    {
                        return;
                    }

                    var sponsorDetails = new FriendDetailsPage { FriendModel = sponsor };

                    await NavigationService.PushAsync(this.Navigation, sponsorDetails);
                    this.ListViewFriends.SelectedItem = null;
                };
        }

        public FriendsViewModel FriendsViewModel =>
            this.friendsViewModel ??= this.BindingContext as FriendsViewModel;

        public override AppPage PageType => AppPage.Friends;

        public void ListViewTapped(object sender, ItemTappedEventArgs e)
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

            if (!this.FriendsViewModel.Friends.Any())
            {
                this.FriendsViewModel.LoadFriendsCommand.Execute(true);
            }

            this.ListViewFriends.ItemTapped += this.ListViewTapped;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            this.ListViewFriends.ItemTapped -= this.ListViewTapped;
        }
    }
}

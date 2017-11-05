using System;
using System.Globalization;
using XamarinEvolve.Clients.Portable.Interfaces;

namespace XamarinEvolve.Clients.UI
{
    using DotNetRu.DataStore.Audit.Models;

    using Xamarin.Forms;

    using XamarinEvolve.Clients.Portable;

    public partial class FriendDetailsPage
    {
        public override AppPage PageType => AppPage.Friend;

        private FriendDetailsViewModel FriendDetailsViewModel => this.friendDetailsViewModel ?? (this.friendDetailsViewModel = this.BindingContext as FriendDetailsViewModel);

        private FriendDetailsViewModel friendDetailsViewModel;

        public FriendDetailsPage()
        {
            this.InitializeComponent();
        }


        public FriendModel FriendModel
        {
            get => this.FriendDetailsViewModel.FriendModel;
            set
            {
                this.BindingContext = new FriendDetailsViewModel(this.Navigation, value);
                this.ItemId = value?.Name;
            }
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            this.friendDetailsViewModel = null;
            var adjust = Device.RuntimePlatform != Device.Android ? 1 : -this.FriendDetailsViewModel.FollowItems.Count + 1;
            this.ListViewFollow.HeightRequest =
                (this.FriendDetailsViewModel.FollowItems.Count * this.ListViewFollow.RowHeight) - adjust;
        }

        private void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
        {
            var k = new CultureInfo("ru");
            DependencyService.Get<ILocalize>().SetLocale(k);
        }
    }
}


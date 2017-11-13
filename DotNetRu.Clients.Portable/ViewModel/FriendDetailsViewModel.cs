using System.Collections.Generic;
using XamarinEvolve.Clients.UI;

namespace XamarinEvolve.Clients.Portable
{
    using DotNetRu.DataStore.Audit.Models;

    using MvvmHelpers;

    using Xamarin.Forms;

    using XamarinEvolve.Utils;

    public class FriendDetailsViewModel : ViewModelBase
    {
        private MenuItem selectedFollowItem;
        

        public FriendDetailsViewModel(INavigation navigation, FriendModel friendModel)
            : base(navigation)
        {
            this.FriendModel = friendModel;
            this.FollowItems.Add(
                new MenuItem
                    {
                        Name = friendModel.WebsiteUrl.StripUrlForDisplay(),
                        Parameter = friendModel.WebsiteUrl,
                        Icon = "icon_website.png"
                    });
            if (!string.IsNullOrWhiteSpace(friendModel.TwitterUrl))
            {
                var twitterValue = friendModel.TwitterUrl.CleanUpTwitter();

                this.FollowItems.Add(
                    new MenuItem
                        {
                            Name = $"@{twitterValue}",
                            Parameter = "https://twitter.com/" + twitterValue,
                            Icon = "icon_twitter.png"
                        });
            }

            if (!string.IsNullOrWhiteSpace(friendModel.FacebookProfileName))
            {
                var profileName = friendModel.FacebookProfileName.GetLastPartOfUrl();
                var profileDisplayName = profileName;
                long testProfileId;
                if (long.TryParse(profileName, out testProfileId))
                {
                    profileDisplayName = "Facebook";
                }

                this.FollowItems.Add(
                    new MenuItem
                        {
                            Name = profileDisplayName,
                            Parameter = "https://facebook.com/" + profileName,
                            Icon = "icon_facebook.png"
                        });
            }

            if (!string.IsNullOrWhiteSpace(friendModel.LinkedInUrl))
            {
                this.FollowItems.Add(
                    new MenuItem
                        {
                            Name = "LinkedIn",
                            Parameter = friendModel.LinkedInUrl.StripUrlForDisplay(),
                            Icon = "icon_linkedin.png"
                        });
            }
        }

        public FriendModel FriendModel { get; }

        public ObservableRangeCollection<MenuItem> FollowItems { get; } = new ObservableRangeCollection<MenuItem>();

        public MenuItem SelectedFollowItem
        {
            get => this.selectedFollowItem;
            set
            {
                this.selectedFollowItem = value;
                this.OnPropertyChanged();
                if (this.selectedFollowItem == null) return;

                this.LaunchBrowserCommand.Execute(this.selectedFollowItem.Parameter);

                this.SelectedFollowItem = null;
            }
        }
    }
}


using MvvmHelpers;

using Xamarin.Forms;

using XamarinEvolve.Utils;

namespace XamarinEvolve.Clients.Portable
{
    using DotNetRu.DataStore.Audit.Models;

    public class SponsorDetailsViewModel : ViewModelBase
    {
        
        public Sponsor Sponsor { get; }
        public ObservableRangeCollection<MenuItem> FollowItems { get; } = new ObservableRangeCollection<MenuItem>();

        public SponsorDetailsViewModel(INavigation navigation, Sponsor sponsor) : base(navigation)
        {
            this.Sponsor = sponsor;
            this.FollowItems.Add(new MenuItem
                {
                    Name = sponsor.WebsiteUrl.StripUrlForDisplay(),
                    Parameter = sponsor.WebsiteUrl,
                    Icon = "icon_website.png"
                });
			if (!string.IsNullOrWhiteSpace(sponsor.TwitterUrl))
			{
				var twitterValue = sponsor.TwitterUrl.CleanUpTwitter();

			    this.FollowItems.Add(new MenuItem
				{
					Name = $"@{twitterValue}",
					Parameter = "https://twitter.com/" + twitterValue,
					Icon = "icon_twitter.png"
				});
			}

			if (!string.IsNullOrWhiteSpace(sponsor.FacebookProfileName))
			{
				var profileName = sponsor.FacebookProfileName.GetLastPartOfUrl();
				var profileDisplayName = profileName;
				long testProfileId;
				if (long.TryParse(profileName, out testProfileId))
				{
					profileDisplayName = "Facebook";
				}

			    this.FollowItems.Add(new MenuItem
				{
					Name = profileDisplayName,
					Parameter = "https://facebook.com/" + profileName,
					Icon = "icon_facebook.png"
				});
			}

			if (!string.IsNullOrWhiteSpace(sponsor.LinkedInUrl))
			{
			    this.FollowItems.Add(new MenuItem
				{
					Name = "LinkedIn",
					Parameter = sponsor.LinkedInUrl.StripUrlForDisplay(),
					Icon = "icon_linkedin.png"
				});
			}
		}

        MenuItem selectedFollowItem;
        public MenuItem SelectedFollowItem
        {
            get => this.selectedFollowItem;
            set
            {
                this.selectedFollowItem = value;
                this.OnPropertyChanged();
                if (this.selectedFollowItem == null)
                    return;

                this.LaunchBrowserCommand.Execute(this.selectedFollowItem.Parameter);

                this.SelectedFollowItem = null;
            }
        }
    }
}


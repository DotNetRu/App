using System;
using Xamarin.Forms;
using XamarinEvolve.DataObjects;
using FormsToolkit;
using MvvmHelpers;
using XamarinEvolve.Utils;

namespace XamarinEvolve.Clients.Portable
{
    public class SponsorDetailsViewModel : ViewModelBase
    {
        
        public Sponsor Sponsor { get; }
        public ObservableRangeCollection<MenuItem> FollowItems { get; } = new ObservableRangeCollection<MenuItem>();

        public SponsorDetailsViewModel(INavigation navigation, Sponsor sponsor) : base(navigation)
        {
            Sponsor = sponsor;
            FollowItems.Add(new MenuItem
                {
                    Name = sponsor.WebsiteUrl.StripUrlForDisplay(),
                    Parameter = sponsor.WebsiteUrl,
                    Icon = "icon_website.png"
                });
			if (!string.IsNullOrWhiteSpace(sponsor.TwitterUrl))
			{
				var twitterValue = sponsor.TwitterUrl.CleanUpTwitter();

				FollowItems.Add(new MenuItem
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
				Int64 testProfileId;
				if (Int64.TryParse(profileName, out testProfileId))
				{
					profileDisplayName = "Facebook";
				}
				FollowItems.Add(new MenuItem
				{
					Name = profileDisplayName,
					Parameter = "https://facebook.com/" + profileName,
					Icon = "icon_facebook.png"
				});
			}
			if (!string.IsNullOrWhiteSpace(sponsor.LinkedInUrl))
			{
				FollowItems.Add(new MenuItem
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
            get { return selectedFollowItem; }
            set
            {
                selectedFollowItem = value;
                OnPropertyChanged();
                if (selectedFollowItem == null)
                    return;

                LaunchBrowserCommand.Execute(selectedFollowItem.Parameter);

                SelectedFollowItem = null;
            }
        }
    }
}


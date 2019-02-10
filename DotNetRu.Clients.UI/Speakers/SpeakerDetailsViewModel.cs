using System.Collections.Generic;

using DotNetRu.DataStore.Audit.Models;
using DotNetRu.Utils.Helpers;

using FormsToolkit;

using MvvmHelpers;

using MenuItem = DotNetRu.Clients.Portable.Model.MenuItem;

namespace DotNetRu.Clients.Portable.ViewModel
{    
    public class SpeakerDetailsViewModel : ViewModelBase
    {
        private MenuItem selectedFollowItem;

        private TalkModel selectedTalkModel;

        public SpeakerDetailsViewModel(SpeakerModel speakerModel)
        {
            SpeakerModel = speakerModel;

            FillFollows();
        }

        private void FillFollows()
        {
            if (!string.IsNullOrWhiteSpace(SpeakerModel.BlogUrl))
            {
                this.FollowItems.Add(
                    new MenuItem
                    {
                        Name = SpeakerModel.BlogUrl.StripUrlForDisplay(),
                        Parameter = SpeakerModel.BlogUrl,
                        Icon = "icon_blog.png"
                    });
            }

            if (!string.IsNullOrWhiteSpace(SpeakerModel.TwitterUrl))
            {
                var twitterValue = SpeakerModel.TwitterUrl.CleanUpTwitter();

                this.FollowItems.Add(
                    new MenuItem
                    {
                        Name = $"@{twitterValue}",
                        Parameter = "https://twitter.com/" + twitterValue,
                        Icon = "icon_twitter.png"
                    });
            }

            if (!string.IsNullOrWhiteSpace(SpeakerModel.FacebookProfileName))
            {
                var profileName = SpeakerModel.FacebookProfileName.GetLastPartOfUrl();
                var profileDisplayName = profileName;
                if (long.TryParse(profileName, out _))
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

            if (!string.IsNullOrWhiteSpace(SpeakerModel.LinkedInUrl))
            {
                this.FollowItems.Add(
                    new MenuItem
                    {
                        Name = "LinkedIn",
                        Parameter = SpeakerModel.LinkedInUrl.StripUrlForDisplay(),
                        Icon = "icon_linkedin.png"
                    });
            }

            if (!string.IsNullOrWhiteSpace(SpeakerModel.GitHubUrl))
            {
                var gitHubValue = SpeakerModel.GitHubUrl.CleanUpGitHub();

                this.FollowItems.Add(
                    new MenuItem
                    {
                        Name = $"{gitHubValue}",
                        Parameter = SpeakerModel.GitHubUrl.StripUrlForDisplay(),
                        Icon = "icon_github.png"
                    });                
            }
        }

        public SpeakerModel SpeakerModel { get; set; }

        public IEnumerable<TalkModel> Talks => this.SpeakerModel.Talks;

        public ObservableRangeCollection<MenuItem> FollowItems { get; } = new ObservableRangeCollection<MenuItem>();

        public MenuItem SelectedFollowItem
        {
            get => this.selectedFollowItem;

            set
            {
                this.selectedFollowItem = value;
                this.OnPropertyChanged();
                if (this.selectedFollowItem == null)
                {
                    return;
                }

                this.LaunchBrowserCommand.Execute(this.selectedFollowItem.Parameter);

                this.SelectedFollowItem = null;
            }
        }

        public TalkModel SelectedTalkModel
        {
            get => this.selectedTalkModel;

            set
            {
                this.selectedTalkModel = value;
                this.OnPropertyChanged();
                if (this.selectedTalkModel == null)
                {
                    return;
                }

                MessagingService.Current.SendMessage(MessageKeys.NavigateToSession, this.selectedTalkModel);

                this.SelectedTalkModel = null;
            }
        }
    }
}

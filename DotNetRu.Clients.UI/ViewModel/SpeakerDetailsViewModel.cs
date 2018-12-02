namespace DotNetRu.Clients.Portable.ViewModel
{
    using System.Collections.Generic;

    using DotNetRu.DataStore.Audit.Models;
    using DotNetRu.DataStore.Audit.Services;
    using DotNetRu.Utils.Helpers;

    using FormsToolkit;

    using MvvmHelpers;

    using MenuItem = Model.MenuItem;

    public class SpeakerDetailsViewModel : ViewModelBase
    {
        private MenuItem selectedFollowItem;

        private TalkModel selectedTalkModel;

        public SpeakerDetailsViewModel(SpeakerModel speakerModel)
        {
            this.SpeakerModel = speakerModel;

            if (!string.IsNullOrWhiteSpace(speakerModel.BlogUrl))
            {
                this.FollowItems.Add(
                    new MenuItem
                        {
                            Name = speakerModel.BlogUrl.StripUrlForDisplay(),
                            Parameter = speakerModel.BlogUrl,
                            Icon = "icon_blog.png"
                        });
            }

            if (!string.IsNullOrWhiteSpace(speakerModel.TwitterUrl))
            {
                var twitterValue = speakerModel.TwitterUrl.CleanUpTwitter();

                this.FollowItems.Add(
                    new MenuItem
                        {
                            Name = $"@{twitterValue}",
                            Parameter = "https://twitter.com/" + twitterValue,
                            Icon = "icon_twitter.png"
                        });
            }

            if (!string.IsNullOrWhiteSpace(speakerModel.FacebookProfileName))
            {
                var profileName = speakerModel.FacebookProfileName.GetLastPartOfUrl();
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

            if (!string.IsNullOrWhiteSpace(speakerModel.LinkedInUrl))
            {
                this.FollowItems.Add(
                    new MenuItem
                        {
                            Name = "LinkedIn",
                            Parameter = speakerModel.LinkedInUrl.StripUrlForDisplay(),
                            Icon = "icon_linkedin.png"
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

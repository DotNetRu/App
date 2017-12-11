namespace XamarinEvolve.Clients.Portable
{
    using System;
    using System.Linq;
    using System.Windows.Input;

    using DotNetRu.DataStore.Audit.Models;
    using DotNetRu.DataStore.Audit.Services;

    using FormsToolkit;

    using MvvmHelpers;

    using Xamarin.Forms;

    using XamarinEvolve.Utils;
    using XamarinEvolve.Utils.Helpers;

    public class SpeakerDetailsViewModel : ViewModelBase
    {
        private ICommand loadTalksCommand;

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

        public ObservableRangeCollection<TalkModel> Talks { get; } = new ObservableRangeCollection<TalkModel>();

        public ObservableRangeCollection<MenuItem> FollowItems { get; } = new ObservableRangeCollection<MenuItem>();

        public ICommand LoadSessionsCommand => this.loadTalksCommand
                                               ?? (this.loadTalksCommand = new Command(
                                                       this.ExecuteLoadTalksCommand));

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

        public void ExecuteLoadTalksCommand()
        {
            if (this.IsBusy)
            {
                return;
            }

            try
            {
                this.IsBusy = true;

                var talks = TalkService.GetTalks(this.SpeakerModel.Id).OrderBy(talk => talk.StartTime);
                this.Talks.ReplaceRange(talks);
            }
            catch (Exception ex)
            {
                this.Logger.Report(ex);
            }
            finally
            {
                this.IsBusy = false;
            }
        }
    }
}

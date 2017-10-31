namespace XamarinEvolve.Clients.Portable
{
    using System;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using DotNetRu.DataStore.Audit.Models;

    using FormsToolkit;

    using MvvmHelpers;

    using Xamarin.Forms;

    using XamarinEvolve.Utils;
    using XamarinEvolve.Utils.Helpers;

    public class SpeakerDetailsViewModel : ViewModelBase
    {

        public SpeakerModel SpeakerModel { get; set; }

        public ObservableRangeCollection<TalkModel> Sessions { get; } = new ObservableRangeCollection<TalkModel>();

        public ObservableRangeCollection<MenuItem> FollowItems { get; } = new ObservableRangeCollection<MenuItem>();

        private bool hasAdditionalSessions;

        public bool HasAdditionalSessions
        {
            get => this.hasAdditionalSessions;
            set => this.SetProperty(ref this.hasAdditionalSessions, value);
        }

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

        private ICommand loadSessionsCommand;

        public ICommand LoadSessionsCommand => this.loadSessionsCommand
                                               ?? (this.loadSessionsCommand = new Command(
                                                       async () => await this.ExecuteLoadSessionsCommandAsync()));

        public async Task ExecuteLoadSessionsCommandAsync()
        {
            if (this.IsBusy)
            {
                return;
            }

            try
            {
                this.IsBusy = true;

                var items = await this.StoreManager.SessionStore.GetSpeakerSessionsAsync(this.SpeakerModel.Id);

                this.Sessions.ReplaceRange(items);

                this.HasAdditionalSessions = this.Sessions.Count > 0;
            }
            catch (Exception ex)
            {
                this.HasAdditionalSessions = false;
                this.Logger.Report(ex);
            }
            finally
            {
                this.IsBusy = false;
            }
        }

        private MenuItem selectedFollowItem;

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

        private TalkModel selectedTalkModel;

        public TalkModel SelectedTalkModel
        {
            get => this.selectedTalkModel;

            set
            {
                this.selectedTalkModel = value;
                this.OnPropertyChanged();
                if (this.selectedTalkModel == null) return;

                MessagingService.Current.SendMessage(MessageKeys.NavigateToSession, this.selectedTalkModel);

                this.SelectedTalkModel = null;
            }
        }
    }
}


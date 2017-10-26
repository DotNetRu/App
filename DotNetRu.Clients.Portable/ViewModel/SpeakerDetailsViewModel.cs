namespace XamarinEvolve.Clients.Portable
{
    using System;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using DotNetRu.DataStore.Audit.DataObjects;

    using FormsToolkit;

    using MvvmHelpers;

    using Xamarin.Forms;

    using XamarinEvolve.DataObjects;
    using XamarinEvolve.Utils;
    using XamarinEvolve.Utils.Helpers;

    public class SpeakerDetailsViewModel : ViewModelBase
    {

        public Speaker Speaker { get; set; }

        public ObservableRangeCollection<TalkModel> Sessions { get; } = new ObservableRangeCollection<TalkModel>();

        public ObservableRangeCollection<MenuItem> FollowItems { get; } = new ObservableRangeCollection<MenuItem>();

        bool hasAdditionalSessions;

        public bool HasAdditionalSessions
        {
            get => this.hasAdditionalSessions;
            set => this.SetProperty(ref this.hasAdditionalSessions, value);
        }

        private string sessionId;

        public SpeakerDetailsViewModel(Speaker speaker, string sessionId)
            : base()
        {
            this.Speaker = speaker;
            this.sessionId = sessionId;

            if (!string.IsNullOrWhiteSpace(speaker.CompanyWebsiteUrl))
            {
                this.FollowItems.Add(
                    new MenuItem
                        {
                            Name = speaker.CompanyWebsiteUrl.StripUrlForDisplay(),
                            Parameter = speaker.CompanyWebsiteUrl,
                            Icon = "icon_website.png"
                        });
            }

            if (!string.IsNullOrWhiteSpace(speaker.BlogUrl))
            {
                this.FollowItems.Add(
                    new MenuItem
                        {
                            Name = speaker.BlogUrl.StripUrlForDisplay(),
                            Parameter = speaker.BlogUrl,
                            Icon = "icon_blog.png"
                        });
            }

            if (!string.IsNullOrWhiteSpace(speaker.TwitterUrl))
            {
                var twitterValue = speaker.TwitterUrl.CleanUpTwitter();

                this.FollowItems.Add(
                    new MenuItem
                        {
                            Name = $"@{twitterValue}",
                            Parameter = "https://twitter.com/" + twitterValue,
                            Icon = "icon_twitter.png"
                        });
            }

            if (!string.IsNullOrWhiteSpace(speaker.FacebookProfileName))
            {
                var profileName = speaker.FacebookProfileName.GetLastPartOfUrl();
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

            if (!string.IsNullOrWhiteSpace(speaker.LinkedInUrl))
            {
                this.FollowItems.Add(
                    new MenuItem
                        {
                            Name = "LinkedIn",
                            Parameter = speaker.LinkedInUrl.StripUrlForDisplay(),
                            Icon = "icon_linkedin.png"
                        });
            }
        }

        ICommand loadSessionsCommand;

        public ICommand LoadSessionsCommand => this.loadSessionsCommand
                                               ?? (this.loadSessionsCommand = new Command(
                                                       async () => await this.ExecuteLoadSessionsCommandAsync()));

        public async Task ExecuteLoadSessionsCommandAsync()
        {
            if (this.IsBusy) return;

            try
            {
                this.IsBusy = true;

#if DEBUG
                await Task.Delay(1000);
#endif


                var items = await this.StoreManager.SessionStore.GetSpeakerSessionsAsync(this.Speaker.Id);

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

        MenuItem selectedFollowItem;

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

        TalkModel selectedTalkModel;

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


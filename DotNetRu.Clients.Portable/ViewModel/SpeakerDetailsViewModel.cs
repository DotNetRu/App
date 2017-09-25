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

        public ObservableRangeCollection<Session> Sessions { get; } = new ObservableRangeCollection<Session>();

        public ObservableRangeCollection<MenuItem> FollowItems { get; } = new ObservableRangeCollection<MenuItem>();

        bool hasAdditionalSessions;

        public bool HasAdditionalSessions
        {
            get => hasAdditionalSessions;
            set
            {
                SetProperty(ref hasAdditionalSessions, value);
            }
        }

        private string sessionId;

        public SpeakerDetailsViewModel(Speaker speaker, string sessionId)
            : base()
        {
            Speaker = speaker;
            this.sessionId = sessionId;

            if (!string.IsNullOrWhiteSpace(speaker.CompanyWebsiteUrl))
            {
                FollowItems.Add(
                    new MenuItem
                        {
                            Name = speaker.CompanyWebsiteUrl.StripUrlForDisplay(),
                            Parameter = speaker.CompanyWebsiteUrl,
                            Icon = "icon_website.png"
                        });
            }

            if (!string.IsNullOrWhiteSpace(speaker.BlogUrl))
            {
                FollowItems.Add(
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

                FollowItems.Add(
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
                Int64 testProfileId;
                if (Int64.TryParse(profileName, out testProfileId))
                {
                    profileDisplayName = "Facebook";
                }
                FollowItems.Add(
                    new MenuItem
                        {
                            Name = profileDisplayName,
                            Parameter = "https://facebook.com/" + profileName,
                            Icon = "icon_facebook.png"
                        });
            }
            if (!string.IsNullOrWhiteSpace(speaker.LinkedInUrl))
            {
                FollowItems.Add(
                    new MenuItem
                        {
                            Name = "LinkedIn",
                            Parameter = speaker.LinkedInUrl.StripUrlForDisplay(),
                            Icon = "icon_linkedin.png"
                        });
            }
        }

        ICommand loadSessionsCommand;

        public ICommand LoadSessionsCommand => loadSessionsCommand
                                               ?? (loadSessionsCommand = new Command(
                                                       async () => await ExecuteLoadSessionsCommandAsync()));

        public async Task ExecuteLoadSessionsCommandAsync()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;

#if DEBUG
                await Task.Delay(1000);
#endif


                var items = (await StoreManager.SessionStore.GetSpeakerSessionsAsync(Speaker.Id));

                Sessions.ReplaceRange(items);

                HasAdditionalSessions = Sessions.Count > 0;
            }
            catch (Exception ex)
            {
                HasAdditionalSessions = false;
                Logger.Report(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        MenuItem selectedFollowItem;

        public MenuItem SelectedFollowItem
        {
            get
            {
                return selectedFollowItem;
            }
            set
            {
                selectedFollowItem = value;
                OnPropertyChanged();
                if (selectedFollowItem == null) return;

                LaunchBrowserCommand.Execute(selectedFollowItem.Parameter);

                SelectedFollowItem = null;
            }
        }

        Session selectedSession;

        public Session SelectedSession
        {
            get
            {
                return selectedSession;
            }
            set
            {
                selectedSession = value;
                OnPropertyChanged();
                if (selectedSession == null) return;

                MessagingService.Current.SendMessage(MessageKeys.NavigateToSession, selectedSession);

                SelectedSession = null;
            }
        }
    }
}


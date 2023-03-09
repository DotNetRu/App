using System.Collections.Generic;
using DotNetRu.Models.Social;

namespace DotNetRu.Clients.Portable.ViewModel
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using DotNetRu.AppUtils;
    using DotNetRu.AppUtils.Logging;
    using DotNetRu.Clients.Portable.Services;
    using DotNetRu.DataStore.Audit.Models;
    using DotNetRu.Utils.Helpers;

    using FormsToolkit;

    using MvvmHelpers;

    using Xamarin.Forms;

    public class NewsViewModel : ViewModelBase
    {
        private ICommand showConferenceFeedbackCommand;

        private ICommand refreshCommand;

        private Notification notification;

        private bool loadingNotifications;

        private ICommand loadNotificationsCommand;

        private bool loadingSessions;

        private bool noSessions;

        private bool loadingSocial;

        private TalkModel selectedTalkModel;

        private ICommand buyTicketNowCommand;

        private ICommand loadSessionsCommand;

        private ISocialPost selectedSocialPost;

        private bool socialError;

        private ICommand loadSocialCommand;

        public NewsViewModel()
        {
            this.Title = "News";
            this.NextForceRefresh = DateTime.UtcNow.AddMinutes(45);

            MessagingService.Current.Subscribe(
                "conferencefeedback_finished",
                (m) => { Device.BeginInvokeOnMainThread(this.EvaluateVisualState); });
        }

        public ObservableRangeCollection<ISocialPost> SocialPosts { get; set; } = new ObservableRangeCollection<ISocialPost>();

        public ObservableRangeCollection<TalkModel> Sessions { get; } = new ObservableRangeCollection<TalkModel>();

        public DateTime NextForceRefresh { get; set; }

        public ICommand RefreshCommand =>
            this.refreshCommand ??= new Command(async () => await this.ExecuteRefreshCommandAsync());

        public Notification Notification
        {
            get => this.notification;
            set => this.SetProperty(ref this.notification, value);
        }

        public bool LoadingNotifications
        {
            get => this.loadingNotifications;
            set => this.SetProperty(ref this.loadingNotifications, value);
        }

        public ICommand LoadNotificationsCommand =>
            this.loadNotificationsCommand ??= new Command(this.ExecuteLoadNotificationsCommand);

        public bool LoadingSessions
        {
            get => this.loadingSessions;
            set => this.SetProperty(ref this.loadingSessions, value);
        }

        public ICommand LoadSessionsCommand =>
            this.loadSessionsCommand ??= new Command(this.ExecuteLoadSessionsCommandAsync);

        public bool NoSessions
        {
            get => this.noSessions;
            set => this.SetProperty(ref this.noSessions, value);
        }

        public bool LoadingSocial
        {
            get => this.loadingSocial;
            set
            {
                this.SetProperty(ref this.loadingSocial, value);
                this.OnPropertyChanged(nameof(this.ActivityIndicatorVisibility));
                this.OnPropertyChanged(nameof(this.NotLoadingSocial));
            }
        }

        public bool NotLoadingSocial => this.SocialPosts.Any() || !this.LoadingSocial;

        public bool ActivityIndicatorVisibility => !this.SocialPosts.Any() && this.LoadingSocial;

        public bool ShowBuyTicketButton =>
            FeatureFlags.ShowBuyTicketButton && EventInfo.StartOfConference.AddDays(-1) >= DateTime.Now;

        public bool ShowConferenceFeedbackButton => FeatureFlags.ShowConferenceFeedbackButton;

        public string SocialHeader => "Social";

        public ICommand BuyTicketNowCommand =>
            this.buyTicketNowCommand ??= new Command(this.ExecuteBuyTicketNowCommand);

        private void ExecuteBuyTicketNowCommand()
        {
            this.LaunchBrowserCommand.Execute(EventInfo.TicketUrl);
        }

        public ICommand ShowConferenceFeedbackCommand =>
            this.showConferenceFeedbackCommand ??= new Command(this.ExecuteShowConferenceFeedbackCommand);

        private void ExecuteShowConferenceFeedbackCommand()
        {
            MessagingService.Current.SendMessage(MessageKeys.NavigateToConferenceFeedback);
        }

        public ICommand LoadSocialCommand =>
            this.loadSocialCommand ??= new Command(async () => await this.ExecuteLoadSocialPostsCommandAsync());

        public bool SocialError
        {
            get => this.socialError;
            set => this.SetProperty(ref this.socialError, value);
        }

        public ISocialPost SelectedSocialPost
        {
            get => this.selectedSocialPost;
            set
            {
                this.selectedSocialPost = value;
                this.OnPropertyChanged();
                if (this.selectedSocialPost == null)
                {
                    return;
                }

                this.LaunchBrowserCommand.Execute(this.selectedSocialPost.Url);

                this.SelectedSocialPost = null;
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

        public void EvaluateVisualState()
        {
            this.OnPropertyChanged(nameof(this.ShowBuyTicketButton));
            this.OnPropertyChanged(nameof(this.ShowConferenceFeedbackButton));
        }

        private void ExecuteLoadSessionsCommandAsync()
        {
            if (this.LoadingSessions)
            {
                return;
            }

            this.LoadingSessions = true;

            try
            {
                this.NoSessions = false;
                this.Sessions.Clear();
                this.OnPropertyChanged(nameof(Sessions));

                // var sessions = await this.StoreManager.SessionStore.GetNextSessions(2);
                // if (sessions != null) this.Sessions.AddRange(sessions);
                this.NoSessions = this.Sessions.Count == 0;
            }
            catch (Exception ex)
            {
                ex.Data["method"] = nameof(ExecuteLoadSessionsCommandAsync);
                DotNetRuLogger.Report(ex);
                this.NoSessions = true;
            }
            finally
            {
                this.LoadingSessions = false;
            }
        }

        private void ExecuteLoadNotificationsCommand()
        {
            if (this.LoadingNotifications)
            {
                return;
            }

            this.LoadingNotifications = true;
            try
            {
            }
            catch (Exception ex)
            {
                ex.Data["method"] = nameof(ExecuteLoadNotificationsCommand);
                DotNetRuLogger.Report(ex);
            }
            finally
            {
                this.LoadingNotifications = false;
            }
        }

        private async Task ExecuteRefreshCommandAsync()
        {
            try
            {
                this.NextForceRefresh = DateTime.UtcNow.AddMinutes(45);
                this.IsBusy = true;

                await this.ExecuteLoadSocialPostsCommandAsync();
            }
            catch (Exception ex)
            {
                ex.Data["method"] = nameof(ExecuteRefreshCommandAsync);
                DotNetRuLogger.Report(ex);
            }
            finally
            {
                this.IsBusy = false;
            }
        }

        private async Task ExecuteLoadSocialPostsCommandAsync()
        {
            if (this.LoadingSocial)
            {
                return;
            }

            this.LoadingSocial = true;
            try
            {
                this.SocialError = false;
                var socialPosts = new List<ISocialPost>();

                var tweets = await TweetService.GetTweetsAsync(Helpers.Settings.CommunitySubscriptions
                    .Where(x => x.IsSelected && x.Type == SocialMediaType.Twitter).Select(x => x.Community.Name)
                    .ToList());
                socialPosts.AddRange(tweets);

                var vkontaktePosts = await VkontakteService.GetVkPostsAsync(Helpers.Settings
                    .CommunitySubscriptions
                    .Where(x => x.IsSelected && x.Type == SocialMediaType.Vkontakte && x.LoadedPosts > 0)
                    .ToDictionary(x => x.Community.Name, x => x.LoadedPosts));
                socialPosts.AddRange(vkontaktePosts);

                this.SocialPosts.ReplaceRange(socialPosts.OrderByDescending(x => x.CreatedDate));
            }
            catch (Exception ex)
            {
                this.SocialError = true;
                ex.Data["method"] = nameof(ExecuteLoadSocialPostsCommandAsync);
                DotNetRuLogger.Report(ex);
            }
            finally
            {
                this.LoadingSocial = false;
            }
        }
    }
}

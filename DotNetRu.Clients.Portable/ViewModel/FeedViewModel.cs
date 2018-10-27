namespace XamarinEvolve.Clients.Portable.ViewModel
{
    using System;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using FormsToolkit;

    using MvvmHelpers;

    using Xamarin.Forms;

    using XamarinEvolve.DataObjects;
    using XamarinEvolve.Utils;
    using XamarinEvolve.Utils.Helpers;

    /// <inheritdoc />
    /// <summary>
    /// The feed view model.
    /// </summary>
    public class FeedViewModel : ViewModelBase
    {
        public ObservableRangeCollection<Tweet> Tweets { get; } = new ObservableRangeCollection<Tweet>();

        public ObservableRangeCollection<Session> Sessions { get; } = new ObservableRangeCollection<Session>();

        public DateTime NextForceRefresh { get; set; }

        public FeedViewModel()
        {
            this.Title = $"{EventInfo.EventName}";
            this.NextForceRefresh = DateTime.UtcNow.AddMinutes(45);

            MessagingService.Current.Subscribe(
                "conferencefeedback_finished",
                (m) => { Device.BeginInvokeOnMainThread(this.EvaluateVisualState); });
        }

        // only start showing upcoming favorites 1 day before the conference
        public bool ShowUpcomingFavorites => EventInfo.StartOfConference.AddDays(-1) < DateTime.UtcNow;

        ICommand refreshCommand;

        public ICommand RefreshCommand =>
            this.refreshCommand ?? (this.refreshCommand = new Command(async () => await this.ExecuteRefreshCommandAsync()));

        async Task ExecuteRefreshCommandAsync()
        {
            try
            {
                this.NextForceRefresh = DateTime.UtcNow.AddMinutes(45);
                this.IsBusy = true;
                var tasks = new Task[]
                                {
                                    this.ExecuteLoadNotificationsCommandAsync(), this.ExecuteLoadSocialCommandAsync(),
                                    this.ExecuteLoadSessionsCommandAsync()
                                };

                await Task.WhenAll(tasks);
            }
            catch (Exception ex)
            {
                ex.Data["method"] = "ExecuteRefreshCommandAsync";
                this.Logger.Report(ex);
            }
            finally
            {
                this.IsBusy = false;
            }
        }

        Notification notification;

        public Notification Notification
        {
            get
            {
                return this.notification;
            }
            set
            {
                this.SetProperty(ref this.notification, value);
            }
        }

        bool loadingNotifications;

        public bool LoadingNotifications
        {
            get
            {
                return this.loadingNotifications;
            }
            set
            {
                this.SetProperty(ref this.loadingNotifications, value);
            }
        }

        ICommand loadNotificationsCommand;

        public ICommand LoadNotificationsCommand => this.loadNotificationsCommand
                                                    ?? (this.loadNotificationsCommand = new Command(
                                                            async () => await this.ExecuteLoadNotificationsCommandAsync()));

        async Task ExecuteLoadNotificationsCommandAsync()
        {
            if (this.LoadingNotifications) return;
            this.LoadingNotifications = true;
#if DEBUG
			await Task.Delay(1000);
#endif

            try
            {
                this.Notification = await this.StoreManager.NotificationStore.GetLatestNotification();
            }
            catch (Exception ex)
            {
                ex.Data["method"] = "ExecuteLoadNotificationsCommandAsync";
                this.Logger.Report(ex);
                this.Notification = new Notification { Date = DateTime.UtcNow, Text = $"Welcome to {EventInfo.EventName}!" };
            }
            finally
            {
                this.LoadingNotifications = false;
            }
        }

        bool loadingSessions;

        public bool LoadingSessions
        {
            get
            {
                return this.loadingSessions;
            }
            set
            {
                this.SetProperty(ref this.loadingSessions, value);
            }
        }

        ICommand loadSessionsCommand;

        public ICommand LoadSessionsCommand => this.loadSessionsCommand
                                               ?? (this.loadSessionsCommand = new Command(
                                                       async () => await this.ExecuteLoadSessionsCommandAsync()));

        async Task ExecuteLoadSessionsCommandAsync()
        {
            if (!this.ShowUpcomingFavorites) return;

            if (this.LoadingSessions) return;

            this.LoadingSessions = true;

            try
            {
                this.NoSessions = false;
                this.Sessions.Clear();
                this.OnPropertyChanged("Sessions");
#if DEBUG
				await Task.Delay(1000);
#endif
                var sessions = await this.StoreManager.SessionStore.GetNextSessions(2);
                if (sessions != null) this.Sessions.AddRange(sessions);

                this.NoSessions = this.Sessions.Count == 0;
            }
            catch (Exception ex)
            {
                ex.Data["method"] = "ExecuteLoadSessionsCommandAsync";
                this.Logger.Report(ex);
                this.NoSessions = true;
            }
            finally
            {
                this.LoadingSessions = false;
            }
        }

        public void EvaluateVisualState()
        {
            this.OnPropertyChanged(nameof(this.ShowBuyTicketButton));
            this.OnPropertyChanged(nameof(this.ShowUpcomingFavorites));
            this.OnPropertyChanged(nameof(this.ShowConferenceFeedbackButton));
        }

        bool noSessions;

        public bool NoSessions
        {
            get
            {
                return this.noSessions;
            }
            set
            {
                this.SetProperty(ref this.noSessions, value);
            }
        }

        Session selectedSession;

        public Session SelectedSession
        {
            get
            {
                return this.selectedSession;
            }
            set
            {
                this.selectedSession = value;
                this.OnPropertyChanged();
                if (this.selectedSession == null) return;

                MessagingService.Current.SendMessage(MessageKeys.NavigateToSession, this.selectedSession);

                this.SelectedSession = null;
            }
        }

        bool loadingSocial;

        public bool LoadingSocial
        {
            get
            {
                return this.loadingSocial;
            }
            set
            {
                this.SetProperty(ref this.loadingSocial, value);
            }
        }

        public bool ShowBuyTicketButton =>
            FeatureFlags.ShowBuyTicketButton && EventInfo.StartOfConference.AddDays(-1) >= DateTime.Now;

        public bool ShowConferenceFeedbackButton => true;

        public string SocialHeader => "Social";

        ICommand shareCommand;

        public ICommand ShareCommand =>
            this.shareCommand ?? (this.shareCommand = new Command(async () => await this.ExecuteShareCommand()));

        async Task ExecuteShareCommand()
        {
            var tweet = DependencyService.Get<ITweetService>();
            await tweet.InitiateConferenceTweet();
        }

        ICommand buyTicketNowCommand;

        public ICommand BuyTicketNowCommand => this.buyTicketNowCommand
                                               ?? (this.buyTicketNowCommand =
                                                       new Command(() => this.ExecuteBuyTicketNowCommand()));

        void ExecuteBuyTicketNowCommand()
        {
            this.LaunchBrowserCommand.Execute(EventInfo.TicketUrl);
        }

        ICommand showConferenceFeedbackCommand;

        public ICommand ShowConferenceFeedbackCommand => this.showConferenceFeedbackCommand
                                                         ?? (this.showConferenceFeedbackCommand = new Command(
                                                                 () => this.ExecuteShowConferenceFeedbackCommand()));

        void ExecuteShowConferenceFeedbackCommand()
        {
            MessagingService.Current.SendMessage(MessageKeys.NavigateToConferenceFeedback);
        }

        ICommand loadSocialCommand;

        public ICommand LoadSocialCommand => this.loadSocialCommand
                                             ?? (this.loadSocialCommand = new Command(
                                                     async () => await this.ExecuteLoadSocialCommandAsync()));

        async Task ExecuteLoadSocialCommandAsync()
        {
            if (this.LoadingSocial) return;

            this.LoadingSocial = true;
            try
            {
                this.SocialError = false;
                this.Tweets.Clear();
                this.Tweets.ReplaceRange(await TweetHelper.Get());
            }
            catch (Exception ex)
            {
                this.SocialError = true;
                ex.Data["method"] = "ExecuteLoadSocialCommandAsync";
                this.Logger.Report(ex);
            }
            finally
            {
                this.LoadingSocial = false;
            }
        }

        bool socialError;

        public bool SocialError
        {
            get
            {
                return this.socialError;
            }
            set
            {
                this.SetProperty(ref this.socialError, value);
            }
        }

        Tweet selectedTweet;

        public Tweet SelectedTweet
        {
            get
            {
                return this.selectedTweet;
            }
            set
            {
                this.selectedTweet = value;
                this.OnPropertyChanged();
                if (this.selectedTweet == null) return;

                this.LaunchBrowserCommand.Execute(this.selectedTweet.Url);

                this.SelectedTweet = null;
            }
        }

        ICommand favoriteCommand;

        public ICommand FavoriteCommand => this.favoriteCommand
                                           ?? (this.favoriteCommand = new Command<Session>(
                                                   async (s) => await this.ExecuteFavoriteCommandAsync(s)));

        async Task ExecuteFavoriteCommandAsync(Session session)
        {
            if (session.IsFavorite)
            {
                MessagingService.Current.SendMessage(
                    MessageKeys.Question,
                    new MessagingServiceQuestion
                        {
                            Negative = "Cancel",
                            Positive = "Unfavorite",
                            Question =
                                "Are you sure you want to remove this session from your favorites?",
                            Title = "Unfavorite Session",
                            OnCompleted = (async (result) =>
                                {
                                    if (!result) return;

                                    await this.ToggleFavorite(session);
                                })
                        });
            }
        }

        async Task ToggleFavorite(Session session)
        {
            var toggled = await this.FavoriteService.ToggleFavorite(session);
            if (toggled)
            {
                await this.ExecuteLoadSessionsCommandAsync();
            }
        }
    }
}


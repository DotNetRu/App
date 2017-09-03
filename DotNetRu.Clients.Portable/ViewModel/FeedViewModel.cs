using System;
using System.Windows.Input;
using System.Threading.Tasks;
using MvvmHelpers;
using System.Linq;
using Xamarin.Forms;
using FormsToolkit;
using System.Reflection;
using PCLStorage;
using Plugin.EmbeddedResource;
using Newtonsoft.Json;
using XamarinEvolve.DataObjects;
using System.Net.Http;
using System.Collections.Generic;
using XamarinEvolve.Utils;
using Plugin.Share;

namespace XamarinEvolve.Clients.Portable
{
	public class FeedViewModel : ViewModelBase
	{

		public ObservableRangeCollection<Tweet> Tweets { get; } = new ObservableRangeCollection<Tweet>();
		public ObservableRangeCollection<Session> Sessions { get; } = new ObservableRangeCollection<Session>();
		public DateTime NextForceRefresh { get; set; }
		public FeedViewModel()
		{
			Title = $"{Utils.EventInfo.EventName}";
			NextForceRefresh = DateTime.UtcNow.AddMinutes(45);

			MessagingService.Current.Subscribe("conferencefeedback_finished", (m) => { Device.BeginInvokeOnMainThread(EvaluateVisualState); });
		}

		// only start showing upcoming favorites 1 day before the conference
		public bool ShowUpcomingFavorites => Utils.EventInfo.StartOfConference.AddDays(-1) < DateTime.UtcNow;

		ICommand refreshCommand;
		public ICommand RefreshCommand =>
				refreshCommand ?? (refreshCommand = new Command(async () => await ExecuteRefreshCommandAsync()));

		async Task ExecuteRefreshCommandAsync()
		{
			try
			{
				NextForceRefresh = DateTime.UtcNow.AddMinutes(45);
				IsBusy = true;
				var tasks = new Task[]
						{
												ExecuteLoadNotificationsCommandAsync(),
												ExecuteLoadSocialCommandAsync(),
												ExecuteLoadSessionsCommandAsync()
						};

				await Task.WhenAll(tasks);
			}
			catch (Exception ex)
			{
				ex.Data["method"] = "ExecuteRefreshCommandAsync";
				Logger.Report(ex);
			}
			finally
			{
				IsBusy = false;
			}
		}

		Notification notification;
		public Notification Notification
		{
			get { return notification; }
			set { SetProperty(ref notification, value); }
		}

		bool loadingNotifications;
		public bool LoadingNotifications
		{
			get { return loadingNotifications; }
			set { SetProperty(ref loadingNotifications, value); }
		}

		ICommand loadNotificationsCommand;
		public ICommand LoadNotificationsCommand =>
				loadNotificationsCommand ?? (loadNotificationsCommand = new Command(async () => await ExecuteLoadNotificationsCommandAsync()));

		async Task ExecuteLoadNotificationsCommandAsync()
		{
			if (LoadingNotifications)
				return;
			LoadingNotifications = true;
#if DEBUG
			await Task.Delay(1000);
#endif

			try
			{
				Notification = await StoreManager.NotificationStore.GetLatestNotification();
			}
			catch (Exception ex)
			{
				ex.Data["method"] = "ExecuteLoadNotificationsCommandAsync";
				Logger.Report(ex);
				Notification = new Notification
				{
					Date = DateTime.UtcNow,
					Text = $"Welcome to {Utils.EventInfo.EventName}!"
				};
			}
			finally
			{
				LoadingNotifications = false;
			}
		}

		bool loadingSessions;
		public bool LoadingSessions
		{
			get { return loadingSessions; }
			set { SetProperty(ref loadingSessions, value); }
		}

		ICommand loadSessionsCommand;
		public ICommand LoadSessionsCommand =>
				loadSessionsCommand ?? (loadSessionsCommand = new Command(async () => await ExecuteLoadSessionsCommandAsync()));

		async Task ExecuteLoadSessionsCommandAsync()
		{
			if (!ShowUpcomingFavorites)
				return;

			if (LoadingSessions)
				return;

			LoadingSessions = true;

			try
			{
				NoSessions = false;
				Sessions.Clear();
				OnPropertyChanged("Sessions");
#if DEBUG
				await Task.Delay(1000);
#endif
				var sessions = await StoreManager.SessionStore.GetNextSessions(2);
				if (sessions != null)
					Sessions.AddRange(sessions);

				NoSessions = Sessions.Count == 0;
			}
			catch (Exception ex)
			{
				ex.Data["method"] = "ExecuteLoadSessionsCommandAsync";
				Logger.Report(ex);
				NoSessions = true;
			}
			finally
			{
				LoadingSessions = false;
			}
		}

		public void EvaluateVisualState()
		{
			OnPropertyChanged(nameof(ShowBuyTicketButton));
			OnPropertyChanged(nameof(ShowUpcomingFavorites));
			OnPropertyChanged(nameof(ShowConferenceFeedbackButton));
		}

		bool noSessions;
		public bool NoSessions
		{
			get { return noSessions; }
			set { SetProperty(ref noSessions, value); }
		}

		Session selectedSession;
		public Session SelectedSession
		{
			get { return selectedSession; }
			set
			{
				selectedSession = value;
				OnPropertyChanged();
				if (selectedSession == null)
					return;

				MessagingService.Current.SendMessage(MessageKeys.NavigateToSession, selectedSession);

				SelectedSession = null;
			}
		}

		bool loadingSocial;
		public bool LoadingSocial
		{
			get { return loadingSocial; }
			set { SetProperty(ref loadingSocial, value); }
		}

		public bool ShowBuyTicketButton => FeatureFlags.ShowBuyTicketButton && Utils.EventInfo.StartOfConference.AddDays(-1) >= DateTime.Now;

#if DEBUG
		public bool ShowConferenceFeedbackButton => true;
#else
		public bool ShowConferenceFeedbackButton => FeatureFlags.ShowConferenceFeedbackButton && Utils.EventInfo.EndOfConference.AddHours(-4) <= DateTime.Now && !Settings.Current.IsConferenceFeedbackFinished();
#endif

		public string SocialHeader => "Social";

	    ICommand shareCommand;
		public ICommand ShareCommand =>
		shareCommand ?? (shareCommand = new Command(async () => await ExecuteShareCommand()));

		async Task ExecuteShareCommand()
		{
			var tweet = DependencyService.Get<ITweetService>();
			await tweet.InitiateConferenceTweet();
		}

		ICommand buyTicketNowCommand;
		public ICommand BuyTicketNowCommand =>
	buyTicketNowCommand ?? (buyTicketNowCommand = new Command(() => ExecuteBuyTicketNowCommand()));

		void ExecuteBuyTicketNowCommand()
		{
			LaunchBrowserCommand.Execute(Utils.EventInfo.TicketUrl);
		}

		ICommand showConferenceFeedbackCommand;
		public ICommand ShowConferenceFeedbackCommand =>
			showConferenceFeedbackCommand ?? (showConferenceFeedbackCommand = new Command(() => ExecuteShowConferenceFeedbackCommand()));

		void ExecuteShowConferenceFeedbackCommand()
		{
			MessagingService.Current.SendMessage(MessageKeys.NavigateToConferenceFeedback);
		}

		ICommand loadSocialCommand;
		public ICommand LoadSocialCommand =>
				loadSocialCommand ?? (loadSocialCommand = new Command(async () => await ExecuteLoadSocialCommandAsync()));

		async Task ExecuteLoadSocialCommandAsync()
		{
			if (LoadingSocial)
				return;

			LoadingSocial = true;
			try
			{
				SocialError = false;
				Tweets.Clear();
				Tweets.ReplaceRange(await TweetHelper.Get());
			}
			catch (Exception ex)
			{
				SocialError = true;
				ex.Data["method"] = "ExecuteLoadSocialCommandAsync";
				Logger.Report(ex);
			}
			finally
			{
				LoadingSocial = false;
			}
		}

		bool socialError;
		public bool SocialError
		{
			get { return socialError; }
			set { SetProperty(ref socialError, value); }
		}

		Tweet selectedTweet;
		public Tweet SelectedTweet
		{
			get { return selectedTweet; }
			set
			{
				selectedTweet = value;
				OnPropertyChanged();
				if (selectedTweet == null)
					return;

				LaunchBrowserCommand.Execute(selectedTweet.Url);

				SelectedTweet = null;
			}
		}

		ICommand favoriteCommand;
		public ICommand FavoriteCommand =>
		favoriteCommand ?? (favoriteCommand = new Command<Session>(async (s) => await ExecuteFavoriteCommandAsync(s)));

		async Task ExecuteFavoriteCommandAsync(Session session)
		{
			if (session.IsFavorite)
			{
				MessagingService.Current.SendMessage(MessageKeys.Question, new MessagingServiceQuestion
				{
					Negative = "Cancel",
					Positive = "Unfavorite",
					Question = "Are you sure you want to remove this session from your favorites?",
					Title = "Unfavorite Session",
					OnCompleted = (async (result) =>
						{
							if (!result)
								return;

							await ToggleFavorite(session);
						})
				});
			}
		}

		async Task ToggleFavorite(Session session)
		{
			var toggled = await FavoriteService.ToggleFavorite(session);
			if (toggled)
			{
				await ExecuteLoadSessionsCommandAsync();
			}
		}
	}
}


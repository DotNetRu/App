using System;
using Xamarin.Forms;
using System.Threading.Tasks;
using XamarinEvolve.DataObjects;
using System.Windows.Input;
using Plugin.Share;
using FormsToolkit;
using XamarinEvolve.Utils;
using MvvmHelpers;
using System.Linq;

namespace XamarinEvolve.Clients.Portable
{
    using DotNetRu.DataStore.Audit.DataObjects;

    using Plugin.Share.Abstractions;

    using XamarinEvolve.Utils.Extensions;
    using XamarinEvolve.Utils.Helpers;

    public class SessionDetailsViewModel : ViewModelBase
    {
        Session session;

        public Session Session
        {
            get
            {
                return session;
            }
            set
            {
                SetProperty(ref session, value);
            }
        }

        public ObservableRangeCollection<MenuItem> SessionMaterialItems { get; } =
            new ObservableRangeCollection<MenuItem>();

        public SessionDetailsViewModel(INavigation navigation, Session session)
            : base(navigation)
        {
            Session = session;
            if (Session.StartTime.HasValue)
                ShowReminder = !Session.StartTime.Value.IsTba()
                               && Session.EndTime.Value.ToUniversalTime() <= DateTime.UtcNow;
            else ShowReminder = false;

#if DEBUG
            if (string.IsNullOrWhiteSpace(session.PresentationUrl)) session.PresentationUrl = "http://www.xamarin.com";

            if (string.IsNullOrWhiteSpace(session.VideoUrl)) session.VideoUrl = "http://www.xamarin.com";
#endif

            if (!string.IsNullOrWhiteSpace(session.PresentationUrl))
            {
                SessionMaterialItems.Add(
                    new MenuItem
                        {
                            Name = "Presentation Slides",
                            Parameter = session.PresentationUrl,
                            Icon = "icon_presentation.png"
                        });
            }

            if (!string.IsNullOrWhiteSpace(session.VideoUrl))
            {
                SessionMaterialItems.Add(
                    new MenuItem { Name = "Session Recording", Parameter = session.VideoUrl, Icon = "icon_video.png" });
            }

            MessagingService.Current.Subscribe<Session>(MessageKeys.SessionFavoriteToggled, UpdateFavoritedSession);
        }

#if DEBUG
        public bool ShowSessionMaterials => true;
#else
		public bool ShowSessionMaterials => Session.EndTime.HasValue && Session.EndTime.Value.ToUniversalTime() <= DateTime.UtcNow && SessionMaterialItems.Any();
		#endif

        MenuItem selectedSessionMaterialItem;

        public MenuItem SelectedSessionMaterialItem
        {
            get
            {
                return selectedSessionMaterialItem;
            }
            set
            {
                selectedSessionMaterialItem = value;
                OnPropertyChanged();
                if (selectedSessionMaterialItem == null) return;

                LaunchBrowserCommand.Execute(selectedSessionMaterialItem.Parameter);

                SelectedSessionMaterialItem = null;
            }
        }

        void UpdateFavoritedSession(IMessagingService service, Session updatedSession)
        {
            if (Session.Id == updatedSession.Id && Session.IsFavorite != updatedSession.IsFavorite)
            {
                Session.IsFavorite = updatedSession.IsFavorite;
            }
        }

        public bool ShowReminder { get; set; }

        bool isReminderSet;

        public bool IsReminderSet
        {
            get
            {
                return isReminderSet;
            }
            set
            {
                SetProperty(ref isReminderSet, value);
            }
        }



        Speaker selectedSpeaker;

        public Speaker SelectedSpeaker
        {
            get
            {
                return selectedSpeaker;
            }
            set
            {
                selectedSpeaker = value;
                OnPropertyChanged();
                if (selectedSpeaker == null) return;

                MessagingService.Current.SendMessage(MessageKeys.NavigateToSpeaker, selectedSpeaker);

                SelectedSpeaker = null;
            }
        }


        ICommand favoriteCommand;

        public ICommand FavoriteCommand => favoriteCommand
                                           ?? (favoriteCommand = new Command(
                                                   async () => await ExecuteFavoriteCommandAsync()));

        async Task ExecuteFavoriteCommandAsync()
        {
            await FavoriteService.ToggleFavorite(Session);
        }

        ICommand reminderCommand;

        public ICommand ReminderCommand => reminderCommand
                                           ?? (reminderCommand = new Command(
                                                   async () => await ExecuteReminderCommandAsync()));

        async Task ExecuteReminderCommandAsync()
        {
            if (!IsReminderSet)
            {
                var result = await ReminderService.AddReminderAsync(
                                 Session.Id,
                                 new Plugin.Calendars.Abstractions.CalendarEvent
                                     {
                                         AllDay = false,
                                         Description = Session.Abstract,
                                         Location =
                                             Session.Room?.Name
                                             ?? string.Empty,
                                         Name = Session.Title,
                                         Start = Session.StartTime.Value,
                                         End = Session.EndTime.Value
                                     });


                if (!result) return;

                Logger.Track(EvolveLoggerKeys.ReminderAdded, "Title", Session.Title);
                IsReminderSet = true;
            }
            else
            {
                var result = await ReminderService.RemoveReminderAsync(Session.Id);
                if (!result) return;
                Logger.Track(EvolveLoggerKeys.ReminderRemoved, "Title", Session.Title);
                IsReminderSet = false;
            }

        }

        ICommand shareCommand;

        public ICommand ShareCommand =>
            shareCommand ?? (shareCommand = new Command(async () => await ExecuteShareCommandAsync()));

        async Task ExecuteShareCommandAsync()
        {
            Logger.Track(EvolveLoggerKeys.Share, "Title", Session.Title);
            var speakerHandles = Session.SpeakerHandles;
            if (!string.IsNullOrEmpty(speakerHandles))
            {
                speakerHandles = " by " + speakerHandles;
            }
            var message = $"Can't wait for {Session.Title}{speakerHandles} at {EventInfo.HashTag}!";

            if (FeatureFlags.AppLinksEnabled)
            {
                message += " " + Session.GetWebUrl();
            }
            await CrossShare.Current.Share(new ShareMessage { Text = message });
        }

        ICommand loadSessionCommand;

        public ICommand LoadSessionCommand => loadSessionCommand
                                              ?? (loadSessionCommand = new Command(
                                                      async () => await ExecuteLoadSessionCommandAsync()));

        public async Task ExecuteLoadSessionCommandAsync()
        {

            if (IsBusy) return;

            try
            {


                IsBusy = true;


                IsReminderSet = await ReminderService.HasReminderAsync(Session.Id);
                Session.FeedbackLeft = await StoreManager.FeedbackStore.LeftFeedback(Session);


            }
            catch (Exception ex)
            {
                Logger.Report(ex, "Method", "ExecuteLoadSessionCommandAsync");
                MessagingService.Current.SendMessage(MessageKeys.Error, ex);
            }
            finally
            {
                IsBusy = false;
            }

        }
    }
}


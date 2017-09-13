using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamarinEvolve.DataStore.Mock
{
    using XamarinEvolve.DataStore.Mock.Abstractions;

    public class StoreManager : IStoreManager
    {
        #region IStoreManager implementation

        public Task<bool> SyncAllAsync(bool syncUserSpecific)
        {
            return Task.FromResult(true);
        }

        public bool IsInitialized { get { return true; }  }
        public Task InitializeAsync()
        {
            return Task.FromResult(true);
        }

        #endregion

        public Task DropEverythingAsync()
        {
            return Task.FromResult(true);
        }

        INotificationStore _notificationStore;
        public INotificationStore NotificationStore => _notificationStore ?? (_notificationStore  = DependencyService.Get<INotificationStore>());

        ICategoryStore _categoryStore;
        public ICategoryStore CategoryStore => _categoryStore ?? (_categoryStore  = DependencyService.Get<ICategoryStore>());

        IFavoriteStore _favoriteStore;
        public IFavoriteStore FavoriteStore => _favoriteStore ?? (_favoriteStore  = DependencyService.Get<IFavoriteStore>());

        IFeedbackStore _feedbackStore;
        public IFeedbackStore FeedbackStore => _feedbackStore ?? (_feedbackStore  = DependencyService.Get<IFeedbackStore>());

		IConferenceFeedbackStore _conferenceFeedbackStore;
		public IConferenceFeedbackStore ConferenceFeedbackStore => _conferenceFeedbackStore ?? (_conferenceFeedbackStore = DependencyService.Get<IConferenceFeedbackStore>());

        ISessionStore _sessionStore;
        public ISessionStore SessionStore => _sessionStore ?? (_sessionStore  = DependencyService.Get<ISessionStore>());

        ISpeakerStore _speakerStore;
        public ISpeakerStore SpeakerStore => _speakerStore ?? (_speakerStore  = DependencyService.Get<ISpeakerStore>());

        IEventStore _eventStore;
        public IEventStore EventStore => _eventStore ?? (_eventStore = DependencyService.Get<IEventStore>());

        ISponsorStore _sponsorStore;
        public ISponsorStore SponsorStore => _sponsorStore ?? (_sponsorStore  = DependencyService.Get<ISponsorStore>());

    }
}


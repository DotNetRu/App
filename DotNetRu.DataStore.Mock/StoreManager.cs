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

        public bool IsInitialized => true;

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

        public INotificationStore NotificationStore => this._notificationStore ?? (this._notificationStore = DependencyService.Get<INotificationStore>());

        ICategoryStore _categoryStore;

        public ICategoryStore CategoryStore => this._categoryStore ?? (this._categoryStore = DependencyService.Get<ICategoryStore>());

        ISessionStore _sessionStore;

        public ISessionStore SessionStore => this._sessionStore ?? (this._sessionStore = DependencyService.Get<ISessionStore>());

        ISpeakerStore _speakerStore;

        public ISpeakerStore SpeakerStore => this._speakerStore ?? (this._speakerStore = DependencyService.Get<ISpeakerStore>());

        IEventStore _eventStore;

        public IEventStore EventStore => this._eventStore ?? (this._eventStore = DependencyService.Get<IEventStore>());

        ISponsorStore _sponsorStore;

        public ISponsorStore SponsorStore => this._sponsorStore ?? (this._sponsorStore = DependencyService.Get<ISponsorStore>());

    }
}


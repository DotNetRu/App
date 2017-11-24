using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamarinEvolve.DataStore.Mock
{
    using DotNetRu.DataStore.Audit;
    using DotNetRu.DataStore.Audit.Abstractions;

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

        IEventStore _eventStore;

        public IEventStore EventStore => this._eventStore ?? (this._eventStore = DependencyService.Get<IEventStore>());

        IFriendStore friendStore;

        public IFriendStore FriendStore => this.friendStore ?? (this.friendStore = DependencyService.Get<IFriendStore>());

    }
}


namespace DotNetRu.DataStore.Audit
{
    using System.Threading.Tasks;

    using DotNetRu.DataStore.Audit.Abstractions;

    public interface IStoreManager
    {
        bool IsInitialized { get; }
        Task InitializeAsync();
        ICategoryStore CategoryStore { get; }
        ISessionStore SessionStore { get; }
        ISpeakerStore SpeakerStore { get; }
        IFriendStore FriendStore { get; }
        IEventStore EventStore { get; }
        INotificationStore NotificationStore { get; }

        Task<bool> SyncAllAsync(bool syncUserSpecific);
    }
}


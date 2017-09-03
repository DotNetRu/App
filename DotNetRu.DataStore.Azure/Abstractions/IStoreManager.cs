using System.Threading.Tasks;

namespace XamarinEvolve.DataStore.Azure.Abstractions
{
    public interface IStoreManager
    {
        bool IsInitialized { get; }
        Task InitializeAsync();
        ICategoryStore CategoryStore { get; }
        IFavoriteStore FavoriteStore { get; }
        IFeedbackStore FeedbackStore { get; }
		IConferenceFeedbackStore ConferenceFeedbackStore { get; }
        ISessionStore SessionStore { get; }
        ISpeakerStore SpeakerStore { get; }
        ISponsorStore SponsorStore { get; }
        IEventStore EventStore { get; }
        INotificationStore NotificationStore { get; }

        Task<bool> SyncAllAsync(bool syncUserSpecific);
    }
}


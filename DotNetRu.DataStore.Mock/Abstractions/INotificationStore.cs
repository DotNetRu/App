namespace XamarinEvolve.DataStore.Mock.Abstractions
{
    using System.Threading.Tasks;

    using XamarinEvolve.DataObjects;

    public interface INotificationStore : IBaseStore<Notification>
    {
        Task<Notification> GetLatestNotification();
    }
}


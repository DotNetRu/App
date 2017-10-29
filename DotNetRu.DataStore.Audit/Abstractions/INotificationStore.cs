namespace DotNetRu.DataStore.Audit.Abstractions
{
    using System.Threading.Tasks;

    using DotNetRu.DataStore.Audit.Models;

    public interface INotificationStore : IBaseStore<Notification>
    {
        Task<Notification> GetLatestNotification();
    }
}


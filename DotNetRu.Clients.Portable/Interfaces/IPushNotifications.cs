using System.Threading.Tasks;

namespace DotNetRu.Clients.Portable.Interfaces
{
    public interface IPushNotifications
    {
        Task<bool> RegisterForNotifications();

        bool IsRegistered { get; }

        void OpenSettings();
    }
}


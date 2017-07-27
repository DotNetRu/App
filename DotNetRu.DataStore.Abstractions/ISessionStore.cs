using System.Threading.Tasks;
using System.Collections.Generic;
using XamarinEvolve.DataObjects;

namespace XamarinEvolve.DataStore.Abstractions
{
    public interface ISessionStore : IBaseStore<Session>
    {
        Task<IEnumerable<Session>> GetSpeakerSessionsAsync(string speakerId);
        Task<IEnumerable<Session>> GetNextSessions(int maxNumber);
        Task<Session> GetAppIndexSession (string id);
    }
}


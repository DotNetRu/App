namespace XamarinEvolve.DataStore.Mock.Abstractions
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using XamarinEvolve.DataObjects;

    public interface ISessionStore : IBaseStore<Session>
    {
        Task<IEnumerable<Session>> GetSpeakerSessionsAsync(string speakerId);
        Task<IEnumerable<Session>> GetNextSessions(int maxNumber);
        Task<Session> GetAppIndexSession (string id);
    }
}


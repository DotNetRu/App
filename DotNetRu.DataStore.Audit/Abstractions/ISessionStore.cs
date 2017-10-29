namespace DotNetRu.DataStore.Audit.Abstractions
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using DotNetRu.DataStore.Audit.Models;

    public interface ISessionStore : IBaseStore<TalkModel>
    {
        Task<IEnumerable<TalkModel>> GetSpeakerSessionsAsync(string speakerId);
        Task<IEnumerable<TalkModel>> GetNextSessions(int maxNumber);
        Task<TalkModel> GetAppIndexSession (string id);
    }
}


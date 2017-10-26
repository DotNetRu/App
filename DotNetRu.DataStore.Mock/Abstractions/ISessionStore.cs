namespace XamarinEvolve.DataStore.Mock.Abstractions
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using XamarinEvolve.DataObjects;

    public interface ISessionStore : IBaseStore<TalkModel>
    {
        Task<IEnumerable<TalkModel>> GetSpeakerSessionsAsync(string speakerId);
        Task<IEnumerable<TalkModel>> GetNextSessions(int maxNumber);
        Task<TalkModel> GetAppIndexSession (string id);
    }
}


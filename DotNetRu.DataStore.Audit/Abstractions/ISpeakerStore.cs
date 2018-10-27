namespace DotNetRu.DataStore.Audit.Abstractions
{
    using System.Threading.Tasks;

    using DotNetRu.DataStore.Audit.Models;

    public interface ISpeakerStore : IBaseStore<SpeakerModel>
	{
		Task<SpeakerModel> GetAppIndexSpeaker(string id);
	}
}


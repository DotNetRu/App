namespace DotNetRu.DataStore.Audit.Abstractions
{
    using System.Threading.Tasks;

    using DotNetRu.DataStore.Audit.Models;

    public interface ISpeakerStore : IBaseStore<Speaker>
	{
		Task<Speaker> GetAppIndexSpeaker(string id);
	}
}


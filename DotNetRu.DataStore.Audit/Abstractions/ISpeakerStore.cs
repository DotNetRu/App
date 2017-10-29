namespace XamarinEvolve.DataStore.Mock.Abstractions
{
    using System.Threading.Tasks;

    using DotNetRu.DataStore.Audit.DataObjects;

    public interface ISpeakerStore : IBaseStore<Speaker>
	{
		Task<Speaker> GetAppIndexSpeaker(string id);
	}
}


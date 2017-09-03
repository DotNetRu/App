using System.Threading.Tasks;
using XamarinEvolve.DataObjects;

namespace XamarinEvolve.DataStore.Azure.Abstractions
{
	public interface ISpeakerStore : IBaseStore<Speaker>
	{
		Task<Speaker> GetAppIndexSpeaker(string id);
	}
}


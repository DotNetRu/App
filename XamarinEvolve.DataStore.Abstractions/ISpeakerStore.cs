using System.Threading.Tasks;
using XamarinEvolve.DataObjects;

namespace XamarinEvolve.DataStore.Abstractions
{
	public interface ISpeakerStore : IBaseStore<Speaker>
	{
		Task<Speaker> GetAppIndexSpeaker(string id);
	}
}


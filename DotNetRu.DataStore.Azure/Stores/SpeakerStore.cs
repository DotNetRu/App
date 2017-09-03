using System.Threading.Tasks;
using XamarinEvolve.DataObjects;
using XamarinEvolve.DataStore.Azure.Abstractions;

namespace XamarinEvolve.DataStore.Azure.Stores
{
    public class SpeakerStore : BaseStore<Speaker>, ISpeakerStore
    {
        public override string Identifier => "Speaker";

		public async Task<Speaker> GetAppIndexSpeaker(string id)
		{
			await InitializeStore().ConfigureAwait(false);
			var speakers = await Table.Where(s => s.Id == id || s.RemoteId == id).ToListAsync();

			if (speakers == null || speakers.Count == 0)
				return null;

			return speakers[0];
		}

	}
}


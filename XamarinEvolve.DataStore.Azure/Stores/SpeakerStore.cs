using XamarinEvolve.DataStore.Abstractions;
using XamarinEvolve.DataObjects;
using XamarinEvolve.DataStore.Azure;
using System;
using System.Threading.Tasks;

namespace XamarinEvolve.DataStore.Azure
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


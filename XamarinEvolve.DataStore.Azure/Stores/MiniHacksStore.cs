using System;
using System.Threading.Tasks;
using XamarinEvolve.DataObjects;
using XamarinEvolve.DataStore.Abstractions;

namespace XamarinEvolve.DataStore.Azure
{
    public class MiniHacksStore : BaseStore<MiniHack>, IMiniHacksStore
    {
        public MiniHacksStore()
        {
        }

        public override string Identifier => "MiniHacks";

		public async Task<MiniHack> GetAppIndexMiniHack(string id)
		{
			await InitializeStore().ConfigureAwait(false);
			var hacks = await Table.Where(s => s.Id == id || s.RemoteId == id).ToListAsync();

			if (hacks == null || hacks.Count == 0)
				return null;

			return hacks[0];		
		}
	}
}


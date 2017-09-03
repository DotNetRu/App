using System.Threading.Tasks;
using XamarinEvolve.DataObjects;
using XamarinEvolve.DataStore.Azure.Abstractions;

namespace XamarinEvolve.DataStore.Azure.Stores
{
    public class FavoriteStore : BaseStore<Favorite>, IFavoriteStore
    {
        public async  Task<bool> IsFavorite(string sessionId)
        {
            await InitializeStore().ConfigureAwait (false);
            var items = await Table.Where(s => s.SessionId == sessionId).ToListAsync().ConfigureAwait (false);
            return items.Count > 0;
        }

        public Task DropFavorites()
        {
            return Task.FromResult(true);
        }

        public override string Identifier => "Favorite";
    }
}


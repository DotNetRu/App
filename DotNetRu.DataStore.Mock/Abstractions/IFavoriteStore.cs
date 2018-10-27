namespace XamarinEvolve.DataStore.Mock.Abstractions
{
    using System.Threading.Tasks;

    using XamarinEvolve.DataObjects;

    public interface IFavoriteStore : IBaseStore<Favorite>
    {
        Task<bool> IsFavorite(string sessionId);
        Task DropFavorites();
    }
}


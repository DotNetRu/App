namespace DotNetRu.DataStore.Audit.Abstractions
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IBaseStore<T>
    {
        string Identifier { get; }

        Task InitializeStore();

        Task<IEnumerable<T>> GetItemsAsync(bool forceRefresh = false);

        Task<T> GetItemAsync(string id);
    }
}


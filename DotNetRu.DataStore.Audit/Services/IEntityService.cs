namespace DotNetRu.DataStore.Audit.Services
{
    using System.Collections.Generic;

    public interface IEntityService<out T>
    {
        IEnumerable<T> GetItems();
    }
}

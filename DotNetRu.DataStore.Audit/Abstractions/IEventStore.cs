namespace DotNetRu.DataStore.Audit.Abstractions
{
    using DotNetRu.DataStore.Audit.Models;

    public interface IEventStore : IBaseStore<FeaturedEvent>
    {
    }
}


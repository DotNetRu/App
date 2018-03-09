namespace DotNetRu.Clients.Portable.Interfaces
{
    using System.Threading.Tasks;

    using DotNetRu.DataStore.Audit.Models;

    public interface IPlatformSpecificExtension<T>
        where T : IModel
    {
        Task Execute(T entity);

        Task Finish();
    }
}

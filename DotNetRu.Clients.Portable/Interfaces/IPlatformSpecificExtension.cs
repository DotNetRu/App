using System.Threading.Tasks;
using DotNetRu.DataStore.Audit.Models;

namespace DotNetRu.Clients.Portable.Interfaces
{
    public interface IPlatformSpecificExtension<T>
        where T : IModel
    {
        Task Execute(T entity);

        Task Finish();
    }
}

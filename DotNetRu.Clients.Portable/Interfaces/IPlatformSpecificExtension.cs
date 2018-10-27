using System.Threading.Tasks;

namespace XamarinEvolve.Clients.Portable
{
    using DotNetRu.DataStore.Audit.DataObjects;

    public interface IPlatformSpecificExtension<T> where T : IBaseDataObject
	{
		Task Execute(T entity);
		Task Finish();
	}
}

using System.Threading.Tasks;

namespace XamarinEvolve.Clients.Portable
{
	public interface IPlatformSpecificExtension<T> where T : DataObjects.IBaseDataObject
	{
		Task Execute(T entity);
		Task Finish();
	}
}

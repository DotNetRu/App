using XamarinEvolve.DataObjects;

namespace XamarinEvolve.Clients.Portable
{
	public interface IPlatformActionWrapper<T> where T : BaseDataObject
	{
		void Before(T contextEntity);
		void After(T contextEntity);
	}
}

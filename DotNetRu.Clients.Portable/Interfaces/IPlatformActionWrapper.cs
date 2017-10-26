namespace XamarinEvolve.Clients.Portable
{
    using DotNetRu.DataStore.Audit.DataObjects;

    public interface IPlatformActionWrapper<T> where T : BaseDataObject
	{
		void Before(T contextEntity);
		void After(T contextEntity);
	}
}

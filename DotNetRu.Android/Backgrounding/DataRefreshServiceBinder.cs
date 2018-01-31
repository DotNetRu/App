using Android.OS;

namespace DotNetRu.Droid.Backgrounding
{
    public class DataRefreshServiceBinder : Binder
    {
        readonly DataRefreshService service;

        public DataRefreshServiceBinder(DataRefreshService service)
        {
            this.service = service;
        }

        public DataRefreshService GetDemoService()
        {
            return this.service;
        }
    }
}
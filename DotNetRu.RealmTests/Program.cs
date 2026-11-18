using DotNetRu.DataStore.Audit.Services;

namespace DotNetRu.RealmTests
{
    class Program
    {
        public static void Main()
        {
            RealmService.InitializeOfflineDatabase();
        }
    }
}

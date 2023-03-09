using System.Threading.Tasks;
using DotNetRu.AppUtils.Config;
using DotNetRu.DataStore.Audit.Services;

namespace DotNetRu.RealmTests
{
    class Program
    {
        public static async Task Main()
        {
            RealmService.InitializeOfflineDatabase();

            var config = AppConfig.GetConfig();

            await RealmService.InitializeCloudSync(config.RealmServerUrl, config.RealmDatabase);
        }
    }
}

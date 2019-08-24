using DotNetRu.RealmUpdate;
using Microsoft.Extensions.Configuration;
using Realms;
using Realms.Sync;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Conference.RealmUpdate
{
    public class Program
    {
        private static string realmOfflinePath = @"DotNetRu.DataStore.Audit/DotNetRuOffline.realm";

        private static string realmOnlineName = "dotnetru_040819";

        public static async Task Main()
        {
            var stopwatch = Stopwatch.StartNew();
            var auditData = await UpdateManager.GetAuditData();
            stopwatch.Stop();

            Console.WriteLine($"Get data from GitHub time: {stopwatch.Elapsed}");

            var tasks = new[] {
                // UpdateOfflineRealm(auditData),
                UpdateOnlineRealm(auditData)
            };

            await Task.WhenAll(tasks);
        }

        private static async Task UpdateOfflineRealm(AuditData auditData)
        {
            var realmFullPath = $"{Directory.GetCurrentDirectory()}/../../../../{realmOfflinePath}";
            Realm.DeleteRealm(new RealmConfiguration(realmFullPath));

            var realm = Realm.GetInstance(realmFullPath);
            UpdateManager.UpdateRealm(realm, auditData);
        }

        private static async Task UpdateOnlineRealm(AuditData auditData)
        {
            SyncConfigurationBase.LogLevel = LogLevel.Debug;

            var realmUrl = new Uri($"realms://dotnet.de1a.cloud.realm.io/{realmOnlineName}");

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("config.json", optional: true, reloadOnChange: true)
                .Build();

            var user = await User.LoginAsync(Credentials.UsernamePassword(config["Login"], config["Password"], createUser: false), new Uri("https://dotnet.de1a.cloud.realm.io"));

            var tempRealmFile = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            var syncConfiguration = new FullSyncConfiguration(realmUrl, user, tempRealmFile);

            var realm = await Realm.GetInstanceAsync(syncConfiguration);

            UpdateManager.UpdateRealm(realm, auditData);

            await Task.Delay(TimeSpan.FromSeconds(2));
        }
    }
}

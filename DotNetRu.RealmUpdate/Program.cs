using Microsoft.Extensions.Configuration;
using RealmGenerator;
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
            var tasks = new[] {
                UpdateOfflineRealm(),
                // UpdateOnlineRealm()
            };

            await Task.WhenAll(tasks);
        }

        private static async Task UpdateOfflineRealm()
        {
            var stopwatch = Stopwatch.StartNew();

            var auditData = await UpdateManager.GetAuditData();

            stopwatch.Stop();

            Console.WriteLine($"Get data from GitHub time: {stopwatch.Elapsed}");

            var realmFullPath = $"{Directory.GetCurrentDirectory()}/../../../../{realmOfflinePath}";
            Realm.DeleteRealm(new RealmConfiguration(realmFullPath));

            var realm = Realm.GetInstance(realmFullPath);
            using (var transaction = realm.BeginWrite())
            {
                realm.Add(auditData.AuditVersion);

                realm.AddRange(auditData.Speakers);
                realm.AddRange(auditData.Friends);
                realm.AddRange(auditData.Venues);
                realm.AddRange(auditData.Communities);
                realm.AddRange(auditData.Talks);
                realm.AddRange(auditData.Meetups);

                transaction.Commit();
            }
        }

        private static async Task UpdateOnlineRealm()
        {
            var auditData = await UpdateManager.GetAuditData();

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
            using (var transaction = realm.BeginWrite())
            {
                realm.Add(auditData.AuditVersion);

                realm.AddRange(auditData.Speakers);
                realm.AddRange(auditData.Friends);
                realm.AddRange(auditData.Venues);
                realm.AddRange(auditData.Communities);
                realm.AddRange(auditData.Talks);
                realm.AddRange(auditData.Meetups);

                transaction.Commit();
            }

            await Task.Delay(TimeSpan.FromSeconds(30));
        }
    }
}

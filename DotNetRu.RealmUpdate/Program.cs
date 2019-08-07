using DotNetRu.RealmUpdate;
using Microsoft.Extensions.Configuration;
using MoreLinq;
using RealmClone;
using RealmGenerator;
using Realms;
using Realms.Sync;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
            UpdateRealm(realm, auditData);
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

            UpdateRealm(realm, auditData);

            await Task.Delay(TimeSpan.FromSeconds(30));
        }

        private static void UpdateRealm(Realm realm, AuditData auditData)
        {
            using (var transaction = realm.BeginWrite())
            {
                MoveRealmObjects(realm, new[] { auditData.AuditVersion }, x => x.CommitHash);
                MoveRealmObjects(realm, auditData.Communities, x => x.Id);
                MoveRealmObjects(realm, auditData.Friends, x => x.Id);
                MoveRealmObjects(realm, auditData.Meetups, x => x.Id);

                MoveRealmObjects(realm, auditData.Meetups.SelectMany(m => m.Sessions), x => x.Id);

                MoveRealmObjects(realm, auditData.Speakers, x => x.Id);
                MoveRealmObjects(realm, auditData.Talks, x => x.Id);
                MoveRealmObjects(realm, auditData.Venues, x => x.Id);

                transaction.Commit();
            }
        }

        public static void MoveRealmObjects<T, TKey>(Realm realm, IEnumerable<T> newObjects, Func<T, TKey> keySelector) where T : RealmObject
        {
            // TODO use primary key
            var oldObjects = realm.All<T>().ToList();

            var objectsToRemove = oldObjects.ExceptBy(newObjects, keySelector).ToList();

            foreach (var @object in objectsToRemove)
            {
                realm.Remove(@object);
            }
            foreach (var @object in newObjects)
            {
                realm.Add(@object.Clone(), update: true);
            }
        }
    }
}

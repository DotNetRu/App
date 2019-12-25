using DotNetRu.RealmUpdateLibrary;
using Microsoft.Extensions.Configuration;
using Realms;
using Realms.Sync;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Conference.RealmUpdate
{
    public class Program
    {        
        public static async Task Main()
        {
            var auditData = await UpdateManager.GetAuditData();

            UpdateOfflineRealm(auditData);
            await UpdateOnlineRealm(auditData);
        }

        private static void UpdateOfflineRealm(AuditUpdate auditData)
        {
            var realmOfflinePath = @"DotNetRu.DataStore.Audit/DotNetRuOffline.realm";

            var realmFullPath = $"{Directory.GetCurrentDirectory()}/../../../../{realmOfflinePath}";
            Realm.DeleteRealm(new RealmConfiguration(realmFullPath));

            var realm = Realm.GetInstance(realmFullPath);
            DotNetRuRealmHelper.ReplaceRealm(realm, auditData);
        }

        private static async Task UpdateOnlineRealm(AuditUpdate auditData)
        {
            SyncConfigurationBase.LogLevel = LogLevel.Debug;

            var realmURL = "dotnetru.de1a.cloud.realm.io";
            var realmOnlineName = "dotnetru_050919";

            var realmUrl = new Uri($"realms://{realmURL}/{realmOnlineName}");

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("config.json", optional: true, reloadOnChange: true)
                .Build();

            var user = await User.LoginAsync(
                Credentials.UsernamePassword(config["Login"], config["Password"], createUser: false), 
                new Uri($"https://{realmURL}"));

            var tempRealmFile = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            var syncConfiguration = new FullSyncConfiguration(realmUrl, user, tempRealmFile);

            var realm = await Realm.GetInstanceAsync(syncConfiguration);

            DotNetRuRealmHelper.ReplaceRealm(realm, auditData);

            await Task.Delay(TimeSpan.FromSeconds(2));
        }
    }
}

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DotNetRu.DataStore.Audit.RealmModels;
using Realms;
using Realms.Sync;

namespace DotNetRu.AzureService.Helpers
{
    public static class RealmHelper
    {
        internal static async Task<Realm> GetRealm(this User user, Uri realmUrl)
        {
            var tempRealmFile = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            var syncConfiguration = new FullSyncConfiguration(realmUrl, user, tempRealmFile);

            return await Realm.GetInstanceAsync(syncConfiguration);
        }

        internal static async Task<User> GetUser(this RealmSettings realmSettings)
        {
            SyncConfigurationBase.Initialize(UserPersistenceMode.NotEncrypted, basePath: Path.GetTempPath());

            return await User.LoginAsync(
                Credentials.UsernamePassword(realmSettings.Login, realmSettings.Password, createUser: false),
                new Uri($"https://{realmSettings.RealmServerUrl}"));
        }

        internal static string GetCurrentVersion(this Realm realm)
        {
            var auditVersion = realm.All<AuditVersion>();

            return auditVersion.Single().CommitHash;
        }
    }
}

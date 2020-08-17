using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DotNetRu.DataStore.Audit.Helpers;
using DotNetRu.DataStore.Audit.Models;
using DotNetRu.DataStore.Audit.RealmModels;
using DotNetRu.Models.Social;
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

            return await Realms.Sync.User.LoginAsync(
                Credentials.UsernamePassword(realmSettings.Login, realmSettings.Password, createUser: false),
                new Uri($"https://{realmSettings.RealmServerUrl}"));
        }

        internal static string GetCurrentVersion(this Realm realm)
        {
            var auditVersion = realm.All<AuditVersion>();

            return auditVersion.Single().CommitHash;
        }

        internal static async Task<IEnumerable<SubscriptionModel>> GetAllCommunitiesAsync(this RealmSettings realmSettings, SocialMediaType socialMediaType)
        {
            var user = await realmSettings.GetUser();
            var realmUrl = new Uri($"realms://{realmSettings.RealmServerUrl}/{realmSettings.RealmName}");
            var realm = await user.GetRealm(realmUrl);
            return SubscriptionsHelper.GetDefaultCommunitySubscriptionsByRealm(realm)
                .Where(x => x.Type == socialMediaType);
        }
    }
}

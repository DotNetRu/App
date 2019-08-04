using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using MoreLinq;
using RealmClone;
using Realms;
using Realms.Sync;

namespace DotNetRu.DataStore.Audit.Services
{
    public static class RealmHelpers
    {
        public static async Task<User> CreateRealmUser(Uri realmServerUri)
        {
            var user = await User.LoginAsync(Credentials.Anonymous(), realmServerUri);
            Analytics.TrackEvent("Realm user created", new Dictionary<string, string>() { { user.Identity, user.ServerUri.ToString() } });

            return user;
        }

        public static void CloseRealmSafely(Realm realm)
        {
            realm?.Dispose();
        }

        public static async Task RemoveCachedUsers()
        {
            try
            {
                foreach (var user in User.AllLoggedIn)
                {
                    await user.LogOutAsync();
                    Analytics.TrackEvent("Realm user deleted", new Dictionary<string, string>() { { user.Identity, user.ServerUri.ToString() } });
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
        }

        public static void MoveRealmObjects<T, TKey>(Realm fromRealm, Realm toRealm, Func<T, TKey> keySelector) where T : RealmObject
        {
            var newObjects = fromRealm.All<T>().Select(RealmExtensions.Clone).ToList();
            // TODO use primary key
            var oldObjects = toRealm.All<T>().ToList();

            var objectsToRemove = oldObjects.ExceptBy(newObjects, keySelector).ToList();

            foreach (var @object in objectsToRemove)
            {
                toRealm.Remove(@object);
            }
            foreach (var @object in newObjects)
            {
                toRealm.Add(@object, update: true);
            }
        }
    }
}

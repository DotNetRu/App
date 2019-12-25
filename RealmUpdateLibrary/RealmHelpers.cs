using System;
using System.Linq;
using System.Threading.Tasks;
using Realms;
using Realms.Sync;

namespace DotNetRu.RealmUpdateLibrary
{
    public static class RealmHelpers
    {
        public static async Task<User> CreateRealmUser(Uri realmServerUri)
        {
            return await User.LoginAsync(Credentials.Anonymous(), realmServerUri);
        }

        public static async Task<User> GetRealmUser(Uri realmServerUri)
        {
            return User.AllLoggedIn.Any()
                ? User.AllLoggedIn.First()
                : await CreateRealmUser(realmServerUri);
        }

        public static void CloseRealmSafely(Realm realm)
        {
            realm?.Dispose();
        }

        public static async Task RemoveCachedUsers()
        {

            foreach (var user in User.AllLoggedIn)
            {
                await user.LogOutAsync();
            }
        }
    }
}

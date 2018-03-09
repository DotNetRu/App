namespace DotNetRu.DataStore.Audit.Services
{
    using System.Collections.Generic;
    using System.Linq;

    using DotNetRu.DataStore.Audit.Extensions;
    using DotNetRu.DataStore.Audit.Models;
    using DotNetRu.DataStore.Audit.RealmModels;

    public static class FriendService
    {
        private static IEnumerable<FriendModel> friends;

        public static IEnumerable<FriendModel> Friends => friends ?? (friends = GetFriends());

        private static IEnumerable<FriendModel> GetFriends()
        {
            var friends = RealmService.AuditRealm.All<Friend>().ToList();
            return friends.Select(friendEntity => friendEntity.ToModel());
        }
    }
}

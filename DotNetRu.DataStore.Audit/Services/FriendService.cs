namespace DotNetRu.DataStore.Audit.Services
{
    using System.Collections.Generic;
    using System.Linq;

    using DotNetRu.DataStore.Audit.Entities;
    using DotNetRu.DataStore.Audit.Extensions;
    using DotNetRu.DataStore.Audit.Helpers;
    using DotNetRu.DataStore.Audit.Models;

    public static class FriendService
    {
        private static IEnumerable<FriendModel> friends;

        public static IEnumerable<FriendModel> Friends => friends ?? (friends = GetFriends());

        private static IEnumerable<FriendModel> GetFriends()
        {
            var friendEntities = ParseHelper.ParseXml<FriendEntity>("Friends");
            return friendEntities.Select(friendEntity => friendEntity.ToModel());
        }
    }
}

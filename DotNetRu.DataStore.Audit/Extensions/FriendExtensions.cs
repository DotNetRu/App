namespace DotNetRu.DataStore.Audit.Extensions
{
    using System.Linq;

    using DotNetRu.DataStore.Audit.Models;
    using DotNetRu.DataStore.Audit.RealmModels;

    public static class FriendExtensions
    {
        public static FriendModel ToModel(this Friend friend)
        {
            return new FriendModel
                       {
                           Id = friend.Id,
                           Description = friend.Description,
                           Name = friend.Name,
                           WebsiteUrl = friend.Url,
                           Meetups = friend.Meetups.ToList().Select(x => x.ToModel())
                               .OrderBy(x => x.StartTime),
                           LogoSmall = friend.LogoSmall,
                           Logo = friend.Logo
                       };
        }
    }
}

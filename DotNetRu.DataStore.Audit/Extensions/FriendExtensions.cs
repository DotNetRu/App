namespace DotNetRu.DataStore.Audit.Extensions
{
    using System.IO;
    using System.Linq;

    using DotNetRu.DataStore.Audit.Models;
    using DotNetRu.DataStore.Audit.RealmModels;

    using Xamarin.Forms;

    public static class FriendExtensions
    {
        public static FriendModel ToModel(this Friend friend)
        {
            var logoSmallMemoryStream = new MemoryStream(friend.LogoSmall);
            var logoMemoryStream = new MemoryStream(friend.Logo);

            return new FriendModel
                       {
                           Id = friend.Id,
                           Description = friend.Description,
                           Name = friend.Name,
                           WebsiteUrl = friend.Url,
                           Meetups = friend.Meetups.ToList().Select(x => x.ToModel())
                               .OrderBy(x => x.StartTime),
                           LogoSmallImage = ImageSource.FromStream(() => logoSmallMemoryStream),
                           LogoImage = ImageSource.FromStream(() => logoMemoryStream)
                       };
        }
    }
}

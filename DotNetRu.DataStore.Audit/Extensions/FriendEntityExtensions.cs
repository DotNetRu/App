namespace DotNetRu.DataStore.Audit.Extensions
{
    using DotNetRu.DataStore.Audit.Entities;
    using DotNetRu.DataStore.Audit.Models;

    using Xamarin.Forms;

    public static class FriendEntityExtensions
    {
        public static FriendModel ToModel(this FriendEntity friend)
        {
            string imagePath = "DotNetRu.DataStore.Audit.Storage.friends." + friend.Id;

            return new FriendModel
                       {
                           Id = friend.Id,
                           Description = friend.Description,
                           Name = friend.Name,
                           LogoSmallImage = ImageSource.FromResource(imagePath + ".logo.small.png"),
                           LogoImage = ImageSource.FromResource(imagePath + ".logo.png"),
                           WebsiteUrl = friend.Url
                       };
        }
    }
}

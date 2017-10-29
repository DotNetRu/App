namespace DotNetRu.DataStore.Audit.Extensions
{
    using DotNetRu.DataStore.Audit.Entities;
    using DotNetRu.DataStore.Audit.Models;

    using Xamarin.Forms;

    public static class SpeakerEntityExtensions
    {
        public static SpeakerModel ToModel(this SpeakerEntity speaker)
        {
            // PhotoUrl =
            // $@"https://raw.githubusercontent.com/DotNetRu/Audit/master/db/speakers/{
            // speaker.Id
            // }/avatar.jpg",
            // AvatarUrl =
            // $@"https://raw.githubusercontent.com/DotNetRu/Audit/master/db/speakers/{
            // speaker.Id
            // }/avatar.small.jpg",

            // Path to an embedded resource should have '_' instead of '-' 
            string imagePath = "DotNetRu.DataStore.Audit.Storage.speakers." + speaker.Id.Replace("-", "_");
            return new SpeakerModel
                       {
                           Id = speaker.Id,
                           FirstName = speaker.Name,
                           LastName = string.Empty,
                           PhotoImage = ImageSource.FromResource(imagePath + ".avatar.jpg"),
                           AvatarImage = ImageSource.FromResource(imagePath + ".avatar.small.jpg"),
                           CompanyName = speaker.CompanyName,
                           CompanyWebsiteUrl = speaker.CompanyUrl,
                           TwitterUrl = speaker.TwitterUrl,
                           BlogUrl = speaker.BlogUrl,
                           Biography = speaker.Description
                       };
        }
    }
}

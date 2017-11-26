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

            // Display names of embedded resources
            //var assembly = typeof(SpeakerEntity).GetTypeInfo().Assembly;
            //foreach (var res in assembly.GetManifestResourceNames())
            //{
            //    System.Diagnostics.Debug.WriteLine(">>> " + res);
            //}

            string imagePathPrefix = "DotNetRu.DataStore.Audit.Storage.speakers." + speaker.Id;

            string avatarPath = imagePathPrefix + ".avatar.jpg";
            string smallAvatarPath = imagePathPrefix + ".avatar.small.jpg";

            return new SpeakerModel
                       {
                           Id = speaker.Id,
                           FirstName = speaker.Name,
                           LastName = string.Empty,
                           PhotoImage = ImageSource.FromResource(avatarPath),
                           AvatarImage = ImageSource.FromResource(smallAvatarPath),
                           CompanyName = speaker.CompanyName,
                           CompanyWebsiteUrl = speaker.CompanyUrl,
                           TwitterUrl = speaker.TwitterUrl,
                           BlogUrl = speaker.BlogUrl,
                           Biography = speaker.Description
                       };
        }
    }
}

namespace DotNetRu.DataStore.Audit.Extensions
{
    using System.IO;

    using DotNetRu.DataStore.Audit.Models;
    using DotNetRu.DataStore.Audit.RealmModels;
    using DotNetRu.DataStore.Audit.Services;

    using Xamarin.Forms;

    public static class SpeakerExtensions
    {
        public static SpeakerModel ToModel(this Speaker speaker)
        {
            var avatarSmallMemoryStream = new MemoryStream(speaker.AvatarSmall);
            var avatarMemoryStream = new MemoryStream(speaker.Avatar);

            return new SpeakerModel
                       {
                           Id = speaker.Id,
                           FirstName = speaker.Name,
                           LastName = string.Empty,
                           CompanyName = speaker.CompanyName,
                           CompanyWebsiteUrl = speaker.CompanyUrl,
                           TwitterUrl = speaker.TwitterUrl,
                           BlogUrl = speaker.BlogUrl,
                           Biography = speaker.Description,
                           AvatarImage = ImageSource.FromStream(() => avatarSmallMemoryStream),
                           PhotoImage = ImageSource.FromStream(() => avatarMemoryStream)
            };
        }
    }
}

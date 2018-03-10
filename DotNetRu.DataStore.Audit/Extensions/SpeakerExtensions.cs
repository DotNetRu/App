namespace DotNetRu.DataStore.Audit.Extensions
{
    using System.Linq;

    using DotNetRu.DataStore.Audit.Models;
    using DotNetRu.DataStore.Audit.RealmModels;

    public static class SpeakerExtensions
    {
        public static SpeakerModel ToModel(this Speaker speaker)
        {
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
                           AvatarSmall = speaker.AvatarSmall,
                           Avatar = speaker.Avatar,
                           Talks = speaker.Talks.ToList().Select(x => x.ToModel())
                       };
        }
    }
}
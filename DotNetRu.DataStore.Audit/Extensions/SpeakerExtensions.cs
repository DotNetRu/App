using System;
using System.Linq;

using DotNetRu.DataStore.Audit.Models;
using DotNetRu.DataStore.Audit.RealmModels;

namespace DotNetRu.DataStore.Audit.Extensions
{    
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
                           GitHubUrl = speaker.GitHubUrl,
                           BlogUrl = speaker.BlogUrl,
                           Biography = speaker.Description,
                           AvatarSmallURL = new Uri(speaker.AvatarSmallURL),
                           AvatarURL = new Uri(speaker.AvatarURL),
                           Talks = speaker.Talks.ToList().Select(x => x.ToModel())
                       };
        }
    }
}

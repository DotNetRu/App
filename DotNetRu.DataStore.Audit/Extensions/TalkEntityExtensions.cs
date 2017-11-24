namespace DotNetRu.DataStore.Audit.Extensions
{
    using System.Linq;

    using DotNetRu.DataStore.Audit.Entities;
    using DotNetRu.DataStore.Audit.Models;
    using DotNetRu.DataStore.Audit.Services;

    public static class TalkEntityExtensions
    {
        public static TalkModel ToModel(this TalkEntity talkEntity)
        {
            return new TalkModel
                       {
                           TalkId = talkEntity.Id,
                           Title = talkEntity.Title,
                           Abstract = talkEntity.Description,
                           PresentationUrl = talkEntity.SlidesUrl,
                           VideoUrl = talkEntity.VideoUrl,
                           CodeUrl = talkEntity.CodeUrl,
                           ShortTitle = talkEntity.Title,
                           Speakers = SpeakerService.Speakers
                               .Where(s => talkEntity.SpeakerIds.Any(s1 => s1 == s.Id)).ToList()
                       };
        }
    }
}

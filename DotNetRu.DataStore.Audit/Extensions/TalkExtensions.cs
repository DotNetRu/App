using System.Linq;
using DotNetRu.DataStore.Audit.Models;
using DotNetRu.DataStore.Audit.RealmModels;

namespace DotNetRu.DataStore.Audit.Extensions
{    

    public static class TalkExtensions
    {
        public static TalkModel ToModel(this Talk talk)
        {
            return new TalkModel(talk.SeeAlsoTalksIds)
            {
                Id = talk.Id,
                Title = talk.Title,
                Abstract = talk.Description,
                PresentationUrl = talk.SlidesUrl,
                VideoUrl = talk.VideoUrl,
                CodeUrl = talk.CodeUrl,
                ShortTitle = talk.Title,
                Speakers = talk.Speakers.Select(x => x.ToModel()),                                
                Sessions = talk.Sessions.ToList().Select(x => x.ToModel())
            };
        }
    }
}

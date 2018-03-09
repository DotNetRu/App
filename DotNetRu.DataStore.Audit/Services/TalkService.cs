namespace DotNetRu.DataStore.Audit.Services
{
    using System.Collections.Generic;
    using System.Linq;

    using DotNetRu.DataStore.Audit.Extensions;
    using DotNetRu.DataStore.Audit.Models;
    using DotNetRu.DataStore.Audit.RealmModels;

    public static class TalkService
    {
        private static List<TalkModel> talks;

        public static List<TalkModel> Talks => talks ?? (talks = GetTalks().ToList());

        public static IEnumerable<TalkModel> GetTalks(IEnumerable<string> talkIDs)
        {
            return talkIDs.Select(talkID => Talks.Single(talk => talk.TalkId == talkID));
        }

        public static IEnumerable<TalkModel> GetTalks(string speakerId)
        {
            return Talks.Where(t => t.Speakers.Any(s => s.Id == speakerId));
        }

        private static IEnumerable<TalkModel> GetTalks()
        {
            var talkEntities = RealmService.AuditRealm.All<Talk>().ToList();
            return talkEntities.Select(x => x.ToModel());
        }
    }
}

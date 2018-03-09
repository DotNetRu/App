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

        private static IEnumerable<TalkModel> GetTalks()
        {
            var talkEntities = RealmService.AuditRealm.All<Talk>().ToList();
            return talkEntities.Select(x => x.ToModel());
        }
    }
}

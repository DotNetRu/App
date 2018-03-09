namespace DotNetRu.DataStore.Audit.Services
{
    using System.Collections.Generic;
    using System.Linq;

    using DotNetRu.DataStore.Audit.Extensions;
    using DotNetRu.DataStore.Audit.Models;
    using DotNetRu.DataStore.Audit.RealmModels;

    public static class SpeakerService
    {
        private static IEnumerable<SpeakerModel> speakers;

        public static IEnumerable<SpeakerModel> Speakers => speakers ?? (speakers = GetSpeakers());

        private static IEnumerable<SpeakerModel> GetSpeakers()
        {
            var speakers = RealmService.AuditRealm.All<Speaker>().ToList();
            return speakers.Select(speaker => speaker.ToModel());
        }
    }
}
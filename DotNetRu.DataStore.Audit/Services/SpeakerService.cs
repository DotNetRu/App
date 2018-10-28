namespace DotNetRu.DataStore.Audit.Services
{
    using System.Collections.Generic;
    using System.Linq;

    using DotNetRu.DataStore.Audit.Entities;
    using DotNetRu.DataStore.Audit.Extensions;
    using DotNetRu.DataStore.Audit.Helpers;
    using DotNetRu.DataStore.Audit.Models;

    public static class SpeakerService
    {
        private static IEnumerable<SpeakerModel> speakers;

        public static IEnumerable<SpeakerModel> Speakers => speakers ?? (speakers = GetSpeakers());

        private static IEnumerable<SpeakerModel> GetSpeakers()
        {
            var speakerEntities = ParseHelper.ParseXml<SpeakerEntity>("Speakers");
            return speakerEntities.Select(speakerEntity => speakerEntity.ToModel());
        }
    }
}
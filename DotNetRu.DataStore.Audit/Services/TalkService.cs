namespace DotNetRu.DataStore.Audit.Services
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Xml.Serialization;

    using DotNetRu.DataStore.Audit.Entities;
    using DotNetRu.DataStore.Audit.Extensions;
    using DotNetRu.DataStore.Audit.Models;

    public static class TalkService
    {
        public static IEnumerable<TalkModel> GetTalks()
        {
            var assembly = Assembly.Load(new AssemblyName("DotNetRu.DataStore.Audit"));
            var stream = assembly.GetManifestResourceStream("DotNetRu.DataStore.Audit.Storage.talks.xml");
            IEnumerable<TalkEntity> sessions;
            using (var reader = new StreamReader(stream))
            {
                var serializer = new XmlSerializer(typeof(List<TalkEntity>), new XmlRootAttribute("Talks"));
                var deserialized = (List<TalkEntity>)serializer.Deserialize(reader);
                sessions = deserialized;
            }

            return sessions.Select(x => x.ToModel());
        }

        public static IEnumerable<TalkModel> GetTalks(IEnumerable<string> talkIDs)
        {
            var assembly = Assembly.Load(new AssemblyName("DotNetRu.DataStore.Audit"));
            var stream = assembly.GetManifestResourceStream("DotNetRu.DataStore.Audit.Storage.talks.xml");
            IEnumerable<TalkEntity> sessions;
            using (var reader = new StreamReader(stream))
            {
                var serializer = new XmlSerializer(typeof(List<TalkEntity>), new XmlRootAttribute("Talks"));
                var deserialized = (List<TalkEntity>)serializer.Deserialize(reader);

                sessions = deserialized.Where(
                    t => talkIDs.Any(talkID => talkID == t.Id));
            }

            return sessions.Select(x => x.ToModel());
        }

        public static IEnumerable<TalkModel> GetTalks(string speakerId)
        {
            var assembly = Assembly.Load(new AssemblyName("DotNetRu.DataStore.Audit"));
            var stream = assembly.GetManifestResourceStream("DotNetRu.DataStore.Audit.Storage.talks.xml");
            IEnumerable<TalkEntity> talkEntities;
            using (var reader = new StreamReader(stream))
            {
                var serializer = new XmlSerializer(typeof(List<TalkEntity>), new XmlRootAttribute("Talks"));
                var deserialized = (List<TalkEntity>)serializer.Deserialize(reader);

                talkEntities = deserialized.Where(
                    t => t.SpeakerIds.Contains(speakerId));
            }

            return talkEntities.Select(x => x.ToModel());
        }
    }
}

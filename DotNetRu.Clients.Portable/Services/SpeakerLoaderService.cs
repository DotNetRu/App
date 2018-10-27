namespace XamarinEvolve.Clients.Portable
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Xml.Serialization;

    using DotNetRu.DataStore.Audit.Entities;
    using DotNetRu.DataStore.Audit.Extensions;
    using DotNetRu.DataStore.Audit.Models;

    public static class SpeakerLoaderService
    {
        private static IEnumerable<SpeakerModel> speakers;

        public static IEnumerable<SpeakerModel> Speakers => speakers ?? (speakers = GetSpeakers());

        private static IEnumerable<SpeakerModel> GetSpeakers()
        {
            var assembly = Assembly.Load(new AssemblyName("DotNetRu.DataStore.Audit"));
            var stream = assembly.GetManifestResourceStream("DotNetRu.DataStore.Audit.Storage.speakers.xml");
            List<SpeakerEntity> speakerEntities;
            using (var reader = new StreamReader(stream))
            {
                var xRoot = new XmlRootAttribute { ElementName = "Speakers", IsNullable = true };
                var serializer = new XmlSerializer(typeof(List<SpeakerEntity>), xRoot);
                speakerEntities = (List<SpeakerEntity>)serializer.Deserialize(reader);
            }

            return speakerEntities.Select(speakerEntity => speakerEntity.ToModel());
        }
    }
}
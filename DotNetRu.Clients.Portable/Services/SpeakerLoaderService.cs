using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;
using DotNetRu.DataStore.Audit.DataObjects;
using AuditSpeaker = DotNetRu.DataStore.Audit.Models.Speaker;


namespace XamarinEvolve.Clients.Portable
{
    public static class SpeakerLoaderService
    {
        private static List<Speaker> _speakers;

        public static List<Speaker> Speakers
        {
            get
            {
                if (_speakers == null)
                    _speakers = GetSpeakers();
                return _speakers;
            }
        }
        private static List<Speaker> GetSpeakers()
        {
            var assembly = Assembly.Load(new AssemblyName("DotNetRu.DataStore.Audit"));
            var stream = assembly.GetManifestResourceStream("DotNetRu.DataStore.Audit.Storage.speakers.xml");
            List<AuditSpeaker> speakers;
            using (var reader = new StreamReader(stream))
            {
                var xRoot = new XmlRootAttribute
                {
                    ElementName = "Speakers",
                    IsNullable = true
                };
                var serializer = new XmlSerializer(typeof(List<AuditSpeaker>), xRoot);
                speakers = (List<AuditSpeaker>)serializer.Deserialize(reader);
            }
            return AuditSpeakerToUISpeakerConverter(speakers);
        }

        private static List<Speaker> AuditSpeakerToUISpeakerConverter(IEnumerable<AuditSpeaker> auditSpeakers)
        {
            return auditSpeakers.Select(speaker => new Speaker
            {
                Id = speaker.Id,
                FirstName = speaker.Name,
                LastName = "",
                PhotoUrl = $@"https://raw.githubusercontent.com/DotNetRu/Audit/master/db/speakers/{speaker.Id}/avatar.jpg",
                AvatarUrl = $@"https://raw.githubusercontent.com/DotNetRu/Audit/master/db/speakers/{speaker.Id}/avatar.small.jpg",
                CompanyName = speaker.CompanyName,
                CompanyWebsiteUrl = speaker.CompanyUrl,
                TwitterUrl = speaker.TwitterUrl,
                BlogUrl = speaker.BlogUrl,
                Biography = speaker.Description
            }).ToList();
        }
    }
}
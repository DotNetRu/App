namespace XamarinEvolve.Clients.Portable
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Xml.Serialization;

    using DotNetRu.DataStore.Audit.Models;

    using AuditSpeaker = DotNetRu.DataStore.Audit.Entities.Speaker;

    public static class SpeakerLoaderService
    {
        private static List<SpeakerModel> _speakers;

        public static List<SpeakerModel> Speakers => _speakers ?? (_speakers = GetSpeakers());

        private static List<SpeakerModel> GetSpeakers()
        {
            var assembly = Assembly.Load(new AssemblyName("DotNetRu.DataStore.Audit"));
            var stream = assembly.GetManifestResourceStream("DotNetRu.DataStore.Audit.Storage.speakers.xml");
            List<AuditSpeaker> speakers;
            using (var reader = new StreamReader(stream))
            {
                var xRoot = new XmlRootAttribute { ElementName = "Speakers", IsNullable = true };
                var serializer = new XmlSerializer(typeof(List<AuditSpeaker>), xRoot);
                speakers = (List<AuditSpeaker>)serializer.Deserialize(reader);
            }

            return AuditSpeakerToUISpeakerConverter(speakers);
        }

        private static List<SpeakerModel> AuditSpeakerToUISpeakerConverter(IEnumerable<AuditSpeaker> auditSpeakers)
        {
            return auditSpeakers.Select(
                speaker => new SpeakerModel
                               {
                                   Id = speaker.Id,
                                   FirstName = speaker.Name,
                                   LastName = string.Empty,
                                   PhotoUrl =
                                       $@"https://raw.githubusercontent.com/DotNetRu/Audit/master/db/speakers/{
                                               speaker.Id
                                           }/avatar.jpg",
                                   AvatarUrl =
                                       $@"https://raw.githubusercontent.com/DotNetRu/Audit/master/db/speakers/{
                                               speaker.Id
                                           }/avatar.small.jpg",
                                   CompanyName = speaker.CompanyName,
                                   CompanyWebsiteUrl = speaker.CompanyUrl,
                                   TwitterUrl = speaker.TwitterUrl,
                                   BlogUrl = speaker.BlogUrl,
                                   Biography = speaker.Description
                               }).ToList();
        }
    }
}
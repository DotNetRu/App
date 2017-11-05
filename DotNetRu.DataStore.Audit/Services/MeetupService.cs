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

    public static class MeetupService
    {
        public static IEnumerable<MeetupModel> GetMeetups()
        {
            var assembly = Assembly.Load(new AssemblyName("DotNetRu.DataStore.Audit"));
            var stream = assembly.GetManifestResourceStream("DotNetRu.DataStore.Audit.Storage.meetups.xml");
            List<MeetupEntity> meetups;
            using (var reader = new StreamReader(stream))
            {
                var xRoot = new XmlRootAttribute { ElementName = "Meetups", IsNullable = false };
                var serializer = new XmlSerializer(typeof(List<MeetupEntity>), xRoot);
                meetups = (List<MeetupEntity>)serializer.Deserialize(reader);
            }

            return meetups.Select(x => x.ToModel());
        }
    }
}

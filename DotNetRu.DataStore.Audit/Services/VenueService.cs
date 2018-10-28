using System.Collections.Generic;

namespace DotNetRu.DataStore.Audit.Services
{
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Xml.Serialization;

    using DotNetRu.DataStore.Audit.Entities;
    using DotNetRu.DataStore.Audit.Extensions;
    using DotNetRu.DataStore.Audit.Models;

    public static class VenueService
    {
        private static IEnumerable<VenueModel> venues;

        public static IEnumerable<VenueModel> Venues => venues ?? (venues = GetVenues());

        private static IEnumerable<VenueModel> GetVenues()
        {
            var assembly = Assembly.Load(new AssemblyName("DotNetRu.DataStore.Audit"));
            var stream = assembly.GetManifestResourceStream("DotNetRu.DataStore.Audit.Storage.venues.xml");
            List<VenueEntity> venueEntities;
            using (var reader = new StreamReader(stream))
            {
                var xRoot = new XmlRootAttribute { ElementName = "Venues", IsNullable = true };
                var serializer = new XmlSerializer(typeof(List<VenueEntity>), xRoot);
                venueEntities = (List<VenueEntity>)serializer.Deserialize(reader);
            }

            return venueEntities.Select(x => x.ToModel());
        }
    }
}

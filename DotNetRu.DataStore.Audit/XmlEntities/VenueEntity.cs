using System.Xml.Serialization;

namespace DotNetRu.DataStore.Audit.XmlEntities
{
    [XmlType("Venue")]
    public class VenueEntity
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string MapUrl { get; set; }
    }
}
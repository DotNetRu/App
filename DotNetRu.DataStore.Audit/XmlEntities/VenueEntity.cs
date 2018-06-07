namespace DotNetRu.DataStore.Audit.XmlEntities
{
    using System.Xml.Serialization;

    [XmlType("Venue")]
    public class VenueEntity
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string MapUrl { get; set; }
    }
}
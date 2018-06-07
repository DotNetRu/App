namespace DotNetRu.DataStore.Audit.XmlEntities
{
    using System.Xml.Serialization;

    [XmlType("Community")]
    public class CommunityEntity
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string City { get; set; }

        public string TimeZone { get; set; }
    }
}
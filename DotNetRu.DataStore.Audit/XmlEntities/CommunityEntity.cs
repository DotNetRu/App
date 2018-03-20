using System.Xml.Serialization;

namespace DotNetRu.DataStore.Audit.XmlEntities
{
    [XmlType("Community")]
    public class CommunityEntity
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string City { get; set; }

        public string TimeZone { get; set; }
    }
}
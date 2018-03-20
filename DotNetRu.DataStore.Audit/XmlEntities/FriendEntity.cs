using System.Xml.Serialization;

namespace DotNetRu.DataStore.Audit.XmlEntities
{
    [XmlType("Friend")]
    public class FriendEntity
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public string Description { get; set; }
    }
}
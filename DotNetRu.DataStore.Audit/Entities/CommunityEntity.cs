namespace DotNetRu.DataStore.Audit.Entities
{
    using System.Xml.Serialization;

    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public class CommunityEntity
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }
}
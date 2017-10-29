namespace DotNetRu.DataStore.Audit.Entities
{
    using System.Xml.Serialization;

    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public class Friend
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
    }
}
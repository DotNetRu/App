namespace DotNetRu.DataStore.Audit.Entities
{
    using System.Xml.Serialization;

    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public class Speaker
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string CompanyName { get; set; }
        public string CompanyUrl { get; set; }
        public string Description { get; set; }

        public string BlogUrl { get; set; }
        public string TwitterUrl { get; set; }
        public string HabrUrl { get; set; }
        public string ContactsUrl { get; set; }
    }
}
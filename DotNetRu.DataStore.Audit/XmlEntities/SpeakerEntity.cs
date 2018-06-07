namespace DotNetRu.DataStore.Audit.XmlEntities
{
    using System.Xml.Serialization;

    [XmlType("Speaker")]
    public class SpeakerEntity
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
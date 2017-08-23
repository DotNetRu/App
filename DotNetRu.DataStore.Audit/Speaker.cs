using System.Xml.Serialization;

namespace DotNetRu.DataStore.Audit
{
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public class Speaker
    {
        /// <remarks />
        public string Id { get; set; }

        /// <remarks />
        public string Name { get; set; }

        /// <remarks />
        public string CompanyName { get; set; }

        /// <remarks />
        public string CompanyUrl { get; set; }

        /// <remarks />
        public string Description { get; set; }
    }
}
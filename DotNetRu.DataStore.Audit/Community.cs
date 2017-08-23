using System.Xml.Serialization;

namespace DotNetRu.DataStore.Audit
{
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public class Community
    {
        /// <remarks />
        public string Id { get; set; }

        /// <remarks />
        public string Name { get; set; }
    }
}
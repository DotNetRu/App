using System.Xml.Serialization;

namespace DotNetRu.DataStore.Audit.Models
{
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public class Venue
    {
 
        public string Id { get; set; }

 
        public string Name { get; set; }

 
        public string Address { get; set; }

 
        public string MapUrl { get; set; }
    }
}
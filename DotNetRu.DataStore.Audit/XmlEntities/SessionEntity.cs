namespace DotNetRu.DataStore.Audit.XmlEntities
{
    using System;
    using System.Xml.Serialization;

    [XmlType("Session")]
    public class SessionEntity
    {
        public string TalkId { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }
    }
}
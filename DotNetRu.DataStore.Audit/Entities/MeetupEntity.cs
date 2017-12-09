namespace DotNetRu.DataStore.Audit.Entities
{
    using System;
    using System.Xml.Serialization;

    [XmlType("Meetup")]
    public class MeetupEntity
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string CommunityId { get; set; }

        [XmlElement(DataType = "date")]
        public DateTime Date { get; set; }

        [XmlArrayItem("FriendId", IsNullable = false)]
        public string[] FriendIds { get; set; }

        public string VenueId { get; set; }

        [XmlArrayItem("TalkId", IsNullable = false)]
        public string[] TalkIds { get; set; }
    }
}
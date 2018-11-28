namespace DotNetRu.DataStore.Audit.XmlEntities
{
    using System.Xml.Serialization;

    [XmlType("Meetup")]
    public class MeetupEntity
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string CommunityId { get; set; }

        [XmlArrayItem("FriendId", IsNullable = false)]
        public string[] FriendIds { get; set; }

        public string VenueId { get; set; }
        
        public SessionEntity[] Sessions { get; set; }
    }
}
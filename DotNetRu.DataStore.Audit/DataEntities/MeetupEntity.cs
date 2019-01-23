namespace DotNetRu.DataStore.Audit.XmlEntities
{
    public class MeetupEntity
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string CommunityId { get; set; }

        public string[] FriendIds { get; set; }

        public string VenueId { get; set; }
        
        public SessionEntity[] Sessions { get; set; }
    }
}

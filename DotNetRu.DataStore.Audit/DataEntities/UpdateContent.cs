using DotNetRu.Models.XML;

namespace PushNotifications
{
    public class UpdateContent
    {
        public string LatestVersion { get; set; }

        public MeetupEntity[] Meetups { get; set; }

        public TalkEntity[] Talks { get; set; }

        public SpeakerEntity[] Speakers { get; set; }

        public VenueEntity[] Venues { get; set; }

        public FriendEntity[] Friends { get; set; }

        public string[] Photos { get; set; }
    }
}

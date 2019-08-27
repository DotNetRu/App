using System.Collections.Generic;
using System.Linq;
using DotNetRu.Models.XML;

namespace DotNetRu.RealmUpdate
{
    public class AuditXmlUpdate
    {
        public IEnumerable<CommunityEntity> Communities { get; set; }

        public IEnumerable<FriendEntity> Friends { get; set; }

        public IEnumerable<MeetupEntity> Meetups { get; set; }

        public IEnumerable<SpeakerEntity> Speakers { get; set; }

        public IEnumerable<TalkEntity> Talks { get; set; }

        public IEnumerable<VenueEntity> Venues { get; set; }

        public AuditXmlUpdate Concat(AuditXmlUpdate other)
        {
            return new AuditXmlUpdate
            {
                Communities = this.Communities.Concat(other.Communities),
                Friends = this.Friends.Concat(other.Friends),
                Meetups = this.Meetups.Concat(other.Meetups),
                Speakers = this.Speakers.Concat(other.Speakers),
                Talks = this.Talks.Concat(other.Talks),
                Venues = this.Venues.Concat(other.Venues)
            };
        }
    }
}

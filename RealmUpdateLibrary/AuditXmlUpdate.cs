using System.Collections.Generic;
using System.Linq;
using DotNetRu.Models.XML;
using MoreLinq;

namespace DotNetRu.RealmUpdateLibrary
{
    public class AuditXmlUpdate
    {
        public IEnumerable<CommunityEntity> Communities { get; set; }

        public IEnumerable<FriendEntity> Friends { get; set; }

        public IEnumerable<MeetupEntity> Meetups { get; set; }

        public IEnumerable<SpeakerEntity> Speakers { get; set; }

        public IEnumerable<TalkEntity> Talks { get; set; }

        public IEnumerable<VenueEntity> Venues { get; set; }

        public AuditXmlUpdate Merge(AuditXmlUpdate other)
        {
            return new AuditXmlUpdate
            {
                Communities = Communities.Concat(other.Communities).DistinctBy(x => x.Name),
                Friends = Friends.Concat(other.Friends).DistinctBy(x => x.Id),
                Meetups = Meetups.Concat(other.Meetups).DistinctBy(x => x.Id),
                Speakers = Speakers.Concat(other.Speakers).DistinctBy(x => x.Id),
                Talks = Talks.Concat(other.Talks).DistinctBy(x => x.Id),
                Venues = Venues.Concat(other.Venues).DistinctBy(x => x.Id)
            };
        }
    }
}

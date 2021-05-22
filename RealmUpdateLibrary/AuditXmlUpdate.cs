using System.Collections.Generic;
using DotNetRu.Models.XML;

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

        public string FromCommitSha { get; set; }

        public string ToCommitSha { get; set; }
    }
}

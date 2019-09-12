using System.Collections.Generic;
using DotNetRu.DataStore.Audit.RealmModels;

namespace DotNetRu.RealmUpdate
{
    public class AuditUpdate
    {
        public AuditVersion AuditVersion { get; set; }

        public IEnumerable<Community> Communities { get; set; }

        public IEnumerable<Friend> Friends { get; set; }

        public IEnumerable<Meetup> Meetups { get; set; }

        public IEnumerable<Speaker> Speakers { get; set; }

        public IEnumerable<Talk> Talks { get; set; }

        public IEnumerable<Venue> Venues { get; set; }
    }
}

namespace DotNetRu.DataStore.Audit.RealmModels
{
    using System;
    using System.Collections.Generic;

    using Realms;

    public class Meetup : RealmObject
    {
        [PrimaryKey]
        public string Id { get; set; }

        public string Name { get; set; }

        public string CommunityId { get; set; }

        public DateTimeOffset Date { get; set; }

        public IList<Friend> Friends { get; }

        public string VenueId { get; set; }

        public IList<Talk> Talks { get; }
    }
}
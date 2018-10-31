namespace DotNetRu.DataStore.Audit.RealmModels
{
    using System.Collections.Generic;
    using Realms;

    public class Meetup : RealmObject
    {
        [PrimaryKey]
        public string Id { get; set; }

        public string Name { get; set; }

        public string CommunityId { get; set; }

        public IList<Friend> Friends { get; }

        public Venue Venue { get; set; }

        public IList<Session> Sessions { get; }
    }
}
namespace DotNetRu.DataStore.Audit.RealmModels
{
    using System.Linq;

    using Realms;

    public class Friend : RealmObject
    {
        [PrimaryKey]
        public string Id { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public string Description { get; set; }

        public string LogoSmallURL { get; set; }

        public string LogoURL { get; set; }

        [Backlink(nameof(Meetup.Friends))]
        public IQueryable<Meetup> Meetups { get; }
    }
}

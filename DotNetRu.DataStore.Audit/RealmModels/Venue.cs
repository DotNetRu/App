namespace DotNetRu.DataStore.Audit.RealmModels
{
    using Realms;

    public class Venue : RealmObject
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string MapUrl { get; set; }
    }
}
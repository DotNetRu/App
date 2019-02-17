namespace DotNetRu.DataStore.Audit.RealmModels
{
    using Realms;

    public class Community : RealmObject
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string City { get; set; }

        public string TimeZone { get; set; }
    }
}
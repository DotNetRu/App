namespace DotNetRu.DataStore.Audit.RealmModels
{
    using Realms;

    public class Community : RealmObject
    {
        [PrimaryKey]
        public string Id { get; set; }

        public string Name { get; set; }

        public string City { get; set; }

        public string TimeZone { get; set; }

        public string VkUrl { get; set; }

        public string LogoUrl { get; set; }
    }
}

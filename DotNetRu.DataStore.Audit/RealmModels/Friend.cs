namespace DotNetRu.DataStore.Audit.RealmModels
{
    using Realms;

    public class Friend : RealmObject
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public string Description { get; set; }

        public byte[] LogoSmall { get; set; }

        public byte[] Logo { get; set; }
    }
}
namespace DotNetRu.DataStore.Audit.RealmModels
{
    using Realms;

    public class Meetup : RealmObject
    {
        [PrimaryKey]
        public string Id { get; set; }
    }
}

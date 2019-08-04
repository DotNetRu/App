namespace DotNetRu.DataStore.Audit.RealmModels
{
    using Realms;

    public class AuditVersion : RealmObject
    {
        [PrimaryKey]
        public string CommitHash { get; set; }
    }
}

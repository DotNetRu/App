namespace DotNetRu.DataStore.Audit.RealmModels
{
    using Realms;

    public class AuditVersion : RealmObject
    {
        public string CommitHash { get; set; }
    }
}

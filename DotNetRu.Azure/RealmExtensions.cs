namespace RealmGenerator
{
    using System.IO;
    using Realms;

    using Mapper = AutoMapper.Mapper;

    public static class RealmExtensions
    {
        public static void AddEntities<TEntity, TRealmType>(this Realm realm, string auditPath, string folderPath)
            where TRealmType : RealmObject
        {
            foreach (var filePath in Directory.EnumerateFiles(
                Path.Combine(auditPath, "db", folderPath),
                "*.xml",
                SearchOption.AllDirectories))
            {
                var entity = FileHelper.LoadFromFile<TEntity>(filePath);
                var realmObject = Mapper.Map<TRealmType>(entity);

                realm.Write(() => { realm.Add(realmObject); });
            }
        }
    }
}

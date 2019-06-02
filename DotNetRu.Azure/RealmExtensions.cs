namespace RealmGenerator
{
    using System.IO;
    using AutoMapper;
    using Realms;

    public static class RealmExtensions
    {
        public static void AddEntities<TEntity, TRealmType>(this Realm realm, string folderPath, IMapper mapper)
            where TRealmType : RealmObject
        {
            foreach (var filePath in Directory.EnumerateFiles(
                folderPath,
                "*.xml",
                SearchOption.AllDirectories))
            {
                var entity = FileHelper.LoadFromFile<TEntity>(filePath);
                var realmObject = mapper.Map<TRealmType>(entity);

                realm.Write(() => { realm.Add(realmObject); });
            }
        }
    }
}

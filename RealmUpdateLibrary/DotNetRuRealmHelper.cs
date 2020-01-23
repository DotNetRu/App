using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using Realms;

namespace DotNetRu.RealmUpdateLibrary
{
    public static class DotNetRuRealmHelper
    {
        public static void ReplaceRealm(Realm realm, AuditUpdate auditData)
        {
            ReplaceRealmObjects(realm, new[] { auditData.AuditVersion }, x => x.CommitHash);
            ReplaceRealmObjects(realm, auditData.Communities, x => x.Id);
            ReplaceRealmObjects(realm, auditData.Friends, x => x.Id);
            ReplaceRealmObjects(realm, auditData.Meetups, x => x.Id);

            ReplaceRealmObjects(realm, auditData.Meetups.SelectMany(m => m.Sessions), x => x.Id);

            ReplaceRealmObjects(realm, auditData.Speakers, x => x.Id);
            ReplaceRealmObjects(realm, auditData.Talks, x => x.Id);
            ReplaceRealmObjects(realm, auditData.Venues, x => x.Id);
        }

        private static void ReplaceRealmObjects<T, TKey>(Realm realm, IEnumerable<T> newObjects, Func<T, TKey> keySelector) where T : RealmObject
        {
            // TODO use primary key
            var oldObjects = realm.All<T>().ToList();

            var objectsToRemove = oldObjects.ExceptBy(newObjects, keySelector).ToList();

            foreach (var @object in objectsToRemove)
            {
                realm.Write(() => realm.Remove(@object));
            }

            foreach (var @object in newObjects)
            {
                realm.Write(() => realm.Add(@object.Clone(), update: true));
            }
        }
    }
}

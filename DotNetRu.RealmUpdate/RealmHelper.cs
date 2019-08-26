using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using RealmClone;
using Realms;

namespace DotNetRu.RealmUpdate
{
    public class RealmHelper
    {
        public static void UpdateRealm(Realm realm, AuditUpdate auditData)
        {
            using (var transaction = realm.BeginWrite())
            {
                UpdateRealmObjects(realm, new[] { auditData.AuditVersion });
                UpdateRealmObjects(realm, auditData.Communities);
                UpdateRealmObjects(realm, auditData.Friends);
                UpdateRealmObjects(realm, auditData.Meetups);

                UpdateRealmObjects(realm, auditData.Meetups.SelectMany(m => m.Sessions));

                UpdateRealmObjects(realm, auditData.Speakers);
                UpdateRealmObjects(realm, auditData.Talks);
                UpdateRealmObjects(realm, auditData.Venues);

                transaction.Commit();
            }
        }

        public static void ReplaceRealm(Realm realm, AuditUpdate auditData)
        {
            using (var transaction = realm.BeginWrite())
            {
                MoveRealmObjects(realm, new[] { auditData.AuditVersion }, x => x.CommitHash);
                MoveRealmObjects(realm, auditData.Communities, x => x.Id);
                MoveRealmObjects(realm, auditData.Friends, x => x.Id);
                MoveRealmObjects(realm, auditData.Meetups, x => x.Id);

                MoveRealmObjects(realm, auditData.Meetups.SelectMany(m => m.Sessions), x => x.Id);

                MoveRealmObjects(realm, auditData.Speakers, x => x.Id);
                MoveRealmObjects(realm, auditData.Talks, x => x.Id);
                MoveRealmObjects(realm, auditData.Venues, x => x.Id);

                transaction.Commit();
            }
        }

        private static void UpdateRealmObjects<T>(Realm realm, IEnumerable<T> newObjects) where T : RealmObject
        {
            foreach (var @object in newObjects)
            {
                realm.Add(@object.Clone(), update: true);
            }
        }

        private static void MoveRealmObjects<T, TKey>(Realm realm, IEnumerable<T> newObjects, Func<T, TKey> keySelector) where T : RealmObject
        {
            // TODO use primary key
            var oldObjects = realm.All<T>().ToList();

            var objectsToRemove = oldObjects.ExceptBy(newObjects, keySelector).ToList();

            foreach (var @object in objectsToRemove)
            {
                realm.Remove(@object);
            }
            foreach (var @object in newObjects)
            {
                realm.Add(@object.Clone(), update: true);
            }
        }
    }
}

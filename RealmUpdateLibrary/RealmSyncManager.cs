using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Realms;

namespace DotNetRu.RealmUpdateLibrary
{
    public static class RealmSyncManager
    {
        // TODO remove this hack that ensures right update order
        public static readonly IList<string> ObjectList = new List<string>()
        {
            "Activity",
            "Category",
            "Complexity",
            "ConferenceInfo",
            "ConferenceMap",
            "Event",
            "Location",
            "Speaker",
            "Talk",
            "Sponsor"
        };

        public static void UpdateRealm(Realm offline, Realm online)
        {
            Analytics.TrackEvent("Start updating offline realm");

            // block update if online realm is empty
            if (!online.All("ConferenceInfo").Any())
            {
                Crashes.TrackError(new InvalidOperationException("Attempt to remove data from offline realm"), new Dictionary<string, string>
                    {
                        { "Realm fatal error", "true" },
                        { "Error description", $"Attempt to remove data from offline realm" }
                    });
                return;
            }

            using (var transaction = offline.BeginWrite())
            {
                foreach (var objectName in ObjectList)
                {
                    var objectSchema = offline.Schema.Single(s => s.Name == objectName);
                    var keyProperty = objectSchema.Single(property => property.IsPrimaryKey);

                    MoveRealmObjects(fromRealm: online, toRealm: offline, objectSchema.Name, keyProperty.Name);
                }

                transaction.Commit();
            }
        }

        private static void MoveRealmObjects(Realm fromRealm, Realm toRealm, string className, string primaryKeyPropertyName)
        {
            var newObjects = fromRealm.All(className)
                .ToList()
                .Cast<RealmObject>()
                .Select(RealmExtensions.Clone);

            var oldObjects = toRealm.All(className)
                .ToList()
                .Cast<RealmObject>();

            var objectsToRemove = oldObjects.Where(obj =>
            {
                var type = obj.GetType();

                var primaryKeyProperty = type.GetProperty(primaryKeyPropertyName);

                var sourcePropertyValue = primaryKeyProperty.GetValue(obj);

                return !newObjects.Any(newObject => primaryKeyProperty.GetValue(newObject).GetHashCode() == sourcePropertyValue.GetHashCode());
            });

            foreach (var @object in objectsToRemove)
            {
                toRealm.Remove(@object);
            }
            foreach (var @object in newObjects)
            {
                toRealm.Add(@object, update: true);
            }
        }
    }
}

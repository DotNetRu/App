using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Realms;
using Realms.Schema;

namespace DotNetRu.RealmUpdateLibrary
{
    public static class RealmSyncManager
    {
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

            var adjucencyList = GetAdjecencyList(offline.Schema);
            var updateOrder = BreadthFirstSearch(adjucencyList, offline.Schema.First());

            using (var transaction = offline.BeginWrite())
            {
                foreach (var objectSchema in updateOrder)
                {
                    var keyProperty = objectSchema.Single(property => property.IsPrimaryKey);
                    MoveRealmObjects(fromRealm: online, toRealm: offline, objectSchema.Name, keyProperty.Name);
                }

                transaction.Commit();
            }
        }

        private static Dictionary<ObjectSchema, HashSet<ObjectSchema>> GetAdjecencyList(RealmSchema realmSchema)
        {
            var resultDictionary = new Dictionary<ObjectSchema, HashSet<ObjectSchema>>();

            foreach (var objectShema in realmSchema)
            {
                var hashset = new HashSet<ObjectSchema>();
                foreach (var property in objectShema)
                {
                    var type = Type.GetType(property.ObjectType);

                    var objectSchema = realmSchema.SingleOrDefault(obj => obj.Name == property.ObjectType);
                    if (objectSchema != null)
                    {
                        hashset.Add(objectSchema);
                    }
                }

                resultDictionary[objectShema] = hashset;
            }

            return resultDictionary;
        }

        private static HashSet<T> BreadthFirstSearch<T>(Dictionary<T, HashSet<T>> adjacencyList, T start)
        {
            var visited = new HashSet<T>();

            if (!adjacencyList.ContainsKey(start))
                return visited;

            var queue = new Queue<T>();
            queue.Enqueue(start);

            while (queue.Count > 0)
            {
                var vertex = queue.Dequeue();

                if (visited.Contains(vertex))
                    continue;

                visited.Add(vertex);

                foreach (var neighbor in adjacencyList[vertex])
                    if (!visited.Contains(neighbor))
                        queue.Enqueue(neighbor);
            }

            return visited;
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

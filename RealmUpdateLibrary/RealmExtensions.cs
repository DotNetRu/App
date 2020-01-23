using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Realms;

namespace DotNetRu.RealmUpdateLibrary
{
    public static class RealmExtensions
    {
        public static RealmObject Clone(this RealmObject source)
        {
            // If source is null return null
            if (source == null)
            {
                return default;
            }

            var targetType = source.GetType();

            var target = Activator.CreateInstance(targetType);

            // List of skip namespaces
            var skipNamespaces = new List<string>
            {
                typeof(Realm).Namespace
            };

            var collectionNamespace = typeof(List<string>).Namespace;

            var flags = BindingFlags.IgnoreCase | BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance;

            var targetProperties = targetType.GetProperties(flags)
                .Where(x => !x.IsDefined(typeof(IgnoredAttribute), true))
                .Where(x => !x.IsDefined(typeof(BacklinkAttribute), true));

            foreach (var property in targetProperties)
            {
                if (skipNamespaces.Contains(property.DeclaringType.Namespace))
                {
                    continue;
                }

                var propertyInfo = targetType.GetProperty(property.Name, flags);
                if (propertyInfo == null)
                {
                    continue;
                }

                var sourceValue = property.GetValue(source);

                if (sourceValue is RealmObject)
                {
                    var targetValue = (sourceValue as RealmObject).Clone();
                    propertyInfo.SetValue(target, targetValue);

                    continue;
                }

                if (property.PropertyType.Namespace == collectionNamespace && sourceValue != null)
                {
                    var sourceList = sourceValue as IEnumerable;

                    var targetList = property.GetValue(target) as IList;

                    // Enumerate source list and recursively call Clone method on each object
                    foreach (var item in sourceList)
                    {
                        object value;

                        if (item.GetType().IsValueType || item.GetType() == typeof(string))
                        {
                            value = item;
                        }
                        else
                        {
                            value = (item as RealmObject).Clone();
                        }

                        targetList.Add(value);
                    }

                    continue;
                }

                propertyInfo.SetValue(target, sourceValue);
            }

            return target as RealmObject;
        }
    }
}

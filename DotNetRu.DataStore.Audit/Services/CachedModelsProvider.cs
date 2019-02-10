using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace DotNetRu.DataStore.Audit.Services
{
    public static class CachedModelsProvider<TAppModel> 
                  where TAppModel: class, IIdentified        
    {
        private static Dictionary<string, TAppModel> repository;
        private static readonly ConcurrentDictionary<string, List<TAppModel>> cached;

        static CachedModelsProvider()
        {
            cached = new ConcurrentDictionary<string, List<TAppModel>>();
        }

        public static IEnumerable<TAppModel> Get(ICollection<string> ids)
        {
            string key = string.Join("||", ids);
            if (cached.TryGetValue(key, out var result))
            {
                return result;
            }

            if (repository == null)
            {
                repository = Load();
            }

            result = new List<TAppModel>(ids.Count);
            foreach (string id in ids)
            {
                if (repository.TryGetValue(id, out var model))
                {
                    result.Add(model);                    
                }
            }

            cached[key] = result;

            return result;
        }

        private static Dictionary<string, TAppModel> Load()
        {
            return RealmService.Get<TAppModel>().ToDictionary(m => m.Id);
        }
    }
}

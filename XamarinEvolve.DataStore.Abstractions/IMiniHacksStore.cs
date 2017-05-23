using System;
using System.Threading.Tasks;
using XamarinEvolve.DataObjects;

namespace XamarinEvolve.DataStore.Abstractions
{
    public interface IMiniHacksStore : IBaseStore<MiniHack>
    {
		Task<MiniHack> GetAppIndexMiniHack(string id);
    }
}


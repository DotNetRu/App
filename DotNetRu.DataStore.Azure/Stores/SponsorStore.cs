using XamarinEvolve.DataObjects;
using XamarinEvolve.DataStore.Azure.Abstractions;

namespace XamarinEvolve.DataStore.Azure.Stores
{
    public class SponsorStore : BaseStore<Sponsor>, ISponsorStore
    {
        public override string Identifier => "Sponsor";
    }
}


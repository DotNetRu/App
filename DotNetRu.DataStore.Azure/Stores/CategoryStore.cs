using XamarinEvolve.DataObjects;
using XamarinEvolve.DataStore.Azure.Abstractions;

namespace XamarinEvolve.DataStore.Azure.Stores
{
    public class CategoryStore : BaseStore<Category>, ICategoryStore
    {
        public override string Identifier => "Category";
    }
}


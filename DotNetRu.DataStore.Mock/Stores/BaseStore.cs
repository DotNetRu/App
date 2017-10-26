namespace XamarinEvolve.DataStore.Mock.Stores
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using XamarinEvolve.DataStore.Mock.Abstractions;

    public class BaseStore<T> : IBaseStore<T>
    {
        #region IBaseStore implementation

        public void DropTable()
        {
            
        }

        public virtual Task InitializeStore()
        {
            throw new NotImplementedException();
        }

        public virtual Task<IEnumerable<T>> GetItemsAsync(bool forceRefresh = false)
        {
            throw new NotImplementedException();
        }

        public virtual Task<T> GetItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public virtual Task<bool> InsertAsync(T item)
        {
            throw new NotImplementedException();
        }

        public virtual Task<bool> UpdateAsync(T item)
        {
            throw new NotImplementedException();
        }

        public virtual Task<bool> RemoveAsync(T item)
        {
            throw new NotImplementedException();
        }

        public virtual Task<bool> SyncAsync()
        {
            return Task.FromResult(true);
        }

        public string Identifier => "store";
        #endregion
    }
}


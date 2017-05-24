using System;
using XamarinEvolve.DataStore.Abstractions;
using XamarinEvolve.DataObjects;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.WindowsAzure.MobileServices.Sync;
using Microsoft.WindowsAzure.MobileServices;
using Xamarin.Forms;
using System.Diagnostics;
using Plugin.Connectivity;

namespace XamarinEvolve.DataStore.Azure
{
    public class BaseStore<T> : IBaseStore<T> where T : class, IBaseDataObject, new()
    {
        IStoreManager _storeManager;

        public virtual string Identifier => "Items";

        IMobileServiceSyncTable<T> _table;
        protected IMobileServiceSyncTable<T> Table
        {
            get { return _table ?? (_table = StoreManager.MobileService.GetSyncTable<T>()); }
          
        }

        public void DropTable()
        {
            _table = null;
        }

        public BaseStore()
        {
            
        }

        #region IBaseStore implementation

        public async Task InitializeStore()
        {
            if (_storeManager == null)
                _storeManager = DependencyService.Get<IStoreManager>();

            if (!_storeManager.IsInitialized)
                await _storeManager.InitializeAsync().ConfigureAwait(false);
        }

        public virtual async Task<IEnumerable<T>> GetItemsAsync(bool forceRefresh = false)
        {
            await InitializeStore().ConfigureAwait (false);
			if (forceRefresh)
			{
				await SyncAsync().ConfigureAwait(false);
				await PullLatestAsync().ConfigureAwait(false);
			}
			return await Table.ToEnumerableAsync().ConfigureAwait(false);
        }

        public virtual async Task<T> GetItemAsync(string id)
        {
            await InitializeStore().ConfigureAwait(false);
            await PullLatestAsync().ConfigureAwait(false);

            var items = await Table.Where(s => s.Id == id).ToListAsync().ConfigureAwait(false);

            if (items == null || items.Count == 0)
                return null;
            
            return items[0];
        }

        public virtual async Task<bool> InsertAsync(T item)
        {
            await InitializeStore().ConfigureAwait(false);
            await PullLatestAsync().ConfigureAwait (false);
            await Table.InsertAsync(item).ConfigureAwait(false);
            await SyncAsync().ConfigureAwait (false);
            return true;
        }

        public virtual async Task<bool> UpdateAsync(T item)
        {
            await InitializeStore().ConfigureAwait(false);
            await Table.UpdateAsync(item).ConfigureAwait(false);
            await SyncAsync().ConfigureAwait (false);
            return true;
        }

        public virtual async Task<bool> RemoveAsync(T item)
        {
            await InitializeStore().ConfigureAwait(false);
            await PullLatestAsync ().ConfigureAwait (false);
            await Table.DeleteAsync(item).ConfigureAwait(false);
            await SyncAsync().ConfigureAwait (false);
            return true;
        }

        public async Task<bool> PullLatestAsync()
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                Debug.WriteLine("Unable to pull items, we are offline");
                return false;
            }
            try
            {
                await Table.PullAsync($"all{Identifier}", Table.CreateQuery()).ConfigureAwait(false);
            }
			catch (MobileServicePushFailedException pex)
			{
				Debug.WriteLine($"Unable to pull items for {Identifier}, that is alright as we have offline capabilities: {pex}");
				Debug.WriteLine($"Push status: {pex.PushResult.Status}");
				foreach (var error in pex.PushResult.Errors)
				{
					Debug.WriteLine($"--{error.TableName} : {error.RawResult}");
				}
				return false;
			}
			catch (Exception ex)
            {
				Debug.WriteLine($"Unable to pull items for {Identifier}, that is alright as we have offline capabilities: {ex}");
                return false;
            }
            return true;
        }


        public async Task<bool> SyncAsync()
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                Debug.WriteLine("Unable to sync items, we are offline");
                return false;
            }
			try
			{
				await StoreManager.MobileService.SyncContext.PushAsync().ConfigureAwait(false);
				if (!(await PullLatestAsync().ConfigureAwait(false)))
					return false;
			}
			catch (MobileServicePushFailedException pex)
			{
				Debug.WriteLine($"Unable to sync items for {Identifier}, that is alright as we have offline capabilities: {pex}");
				Debug.WriteLine($"Push status: {pex.PushResult.Status}");
				foreach (var error in pex.PushResult.Errors)
				{
					Debug.WriteLine($"--{error.TableName} : {error.RawResult}");
				}
				return false;
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"Unable to sync items for {Identifier}, that is alright as we have offline capabilities: {ex}");
                return false;
            }
            finally
            {
            }
            return true;
        }

        #endregion
    }
}


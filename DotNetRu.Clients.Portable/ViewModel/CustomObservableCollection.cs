using System.Collections.Generic;
using System.Collections.Specialized;
using MvvmHelpers;

namespace DotNetRu.Clients.Portable.ViewModel
{
    public class CustomObservableCollection<T> : ObservableRangeCollection<T>
    {
        public CustomObservableCollection()
        {
        }

        public CustomObservableCollection(IEnumerable<T> items)
            : this()
        {
            foreach (var item in items)
            {
                this.Add(item);
            }
        }

        public void ReportItemChange(T item)
        {
            NotifyCollectionChangedEventArgs args = new NotifyCollectionChangedEventArgs(
                NotifyCollectionChangedAction.Replace,
                item,
                item,
                this.IndexOf(item));
            this.OnCollectionChanged(args);
        }
    }
}
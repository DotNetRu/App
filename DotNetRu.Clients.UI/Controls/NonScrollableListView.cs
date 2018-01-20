namespace DotNetRu.Clients.UI.Controls
{
    using System.Collections.Specialized;
    using System.Linq;

    using Xamarin.Forms;

    public class NonScrollableListView : ListView
    {
        public NonScrollableListView()
            : base(ListViewCachingStrategy.RecycleElement)
        {
            if (Device.RuntimePlatform == Device.UWP)
            {
                this.BackgroundColor = Color.White;
            }

            this.TemplatedItems.CollectionChanged += this.TemplatedItemsOnCollectionChanged;
        }

        private void TemplatedItemsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            this.UpdateListViewHeight();
        }

        private void UpdateListViewHeight()
        {
            if (!this.TemplatedItems.All(cell => cell is ViewCell))
            {
                return;
            }

            double height = 0;
            foreach (Cell cell in this.TemplatedItems)
            {
                var sizeRequest = ((ViewCell)cell).View.Measure(this.Width, double.MaxValue, MeasureFlags.IncludeMargins);

                height += sizeRequest.Request.Height;
            }

            this.HeightRequest = height;
        }
    }
}
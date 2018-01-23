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

        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            var sizeRequest = base.OnMeasure(widthConstraint, heightConstraint);

            if (!this.TemplatedItems.All(cell => cell is ViewCell))
            {
                return sizeRequest;
            }

            double height = this.CalculateHeight();

            sizeRequest.Request = new Size(sizeRequest.Request.Width, height);

            return sizeRequest;
        }

        private void TemplatedItemsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            this.UpdateListViewHeight();
        }

        private double CalculateHeight()
        {
            double height = 0;
            foreach (Cell cell in this.TemplatedItems)
            {
                var cellSizeRequest = ((ViewCell)cell).View.Measure(this.Width, double.MaxValue, MeasureFlags.IncludeMargins);
                // ((ViewCell)cell).View.Measure(this.Width, double.MaxValue, MeasureFlags.IncludeMargins);

                height += cellSizeRequest.Request.Height;
            }

            return height;
        }

        private void UpdateListViewHeight()
        {
            if (!this.TemplatedItems.All(cell => cell is ViewCell))
            {
                return;
            }

            this.HeightRequest = this.CalculateHeight();
        }
    }
}
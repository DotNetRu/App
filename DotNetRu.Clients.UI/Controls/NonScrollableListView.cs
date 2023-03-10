namespace DotNetRu.Clients.UI.Controls
{
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.Maui;

    public class NonScrollableListView : ListView
    {
        private readonly HashSet<object> cells = new HashSet<object>();

        public NonScrollableListView()
            : base(ListViewCachingStrategy.RecycleElement)
        {
            if (Device.RuntimePlatform == Device.UWP)
            {
                this.BackgroundColor = Color.White;
            }
        }

        public void AdjustHeight(ViewCell viewCell)
        {
            if (!this.cells.Contains(viewCell))
            {
                var height = this.CalculateCellHeight(viewCell);
                this.HeightRequest += height;
                this.cells.Add(viewCell);
            }
        }

        public double CalculateCellHeight(ViewCell viewCell)
        {
            var cellSizeRequest = viewCell.View.Measure(this.Width, double.MaxValue, MeasureFlags.IncludeMargins);
            return cellSizeRequest.Request.Height;
        }

        public void UpdateListViewHeight()
        {
            if (!this.TemplatedItems.All(cell => cell is ViewCell))
            {
                return;
            }

            this.HeightRequest = this.CalculateHeight();
        }

        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            return base.OnMeasure(widthConstraint, heightConstraint);
        }

        private double CalculateHeight()
        {
            double height = 0;

            foreach (var cell in this.TemplatedItems)
            {
                var viewCell = (ViewCell)cell;

                height += this.CalculateCellHeight(viewCell);
            }

            return height;
        }

        private void OnItemAppearing(object sender, ItemVisibilityEventArgs itemVisibilityEventArgs)
        {
            var source = itemVisibilityEventArgs.Item;

            foreach (Cell cell in this.TemplatedItems)
            {
                if (cell.BindingContext == source)
                {
                    var viewCell = (ViewCell)cell;
                    if (!this.cells.Contains(source))
                    {
                        this.HeightRequest += this.CalculateCellHeight(viewCell);
                        this.cells.Add(source);
                    }
                }
            }
        }
    }
}
using DotNetRu.DataStore.Audit.Models;
using Microsoft.Maui;

namespace DotNetRu.Clients.UI.Cells
{
    public class CategoryCell: ViewCell
    {
        public CategoryCell ()
        {
            this.Height = Device.RuntimePlatform == Device.UWP ? 50 : 44;
            this.View = new CategoryCellView ();
        }
    }

    public partial class CategoryCellView
    {
        public CategoryCellView()
        {
            this.InitializeComponent();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            if (!(this.BindingContext is Category category))
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(category.Color))
            {
                Grid.SetColumn(this.LabelName, 0);
                Grid.SetColumnSpan(this.LabelName, 2);
            }
        }
    }
}


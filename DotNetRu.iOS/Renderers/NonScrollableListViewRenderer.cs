using DotNetRu.Clients.UI.Controls;
using DotNetRu.iOS.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(NonScrollableListView), typeof(NonScrollableListViewRenderer))]
[assembly: ExportRenderer(typeof(AlwaysScrollView), typeof(AlwaysScrollViewRenderer))]

namespace DotNetRu.iOS.Renderers
{
    public class NonScrollableListViewRenderer : ListViewRenderer
    {
        public static void Initialize()
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<ListView> e)
        {
            base.OnElementChanged(e);
            if (this.Control != null)
            {
                this.Control.ScrollEnabled = false;
            }            
        }
    }
}


using Android.Content;
using DotNetRu.Clients.UI.Controls;
using DotNetRu.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(NonScrollableListView), typeof(NonScrollableListViewRenderer))]
namespace DotNetRu.Droid.Renderers
{
    public class NonScrollableListViewRenderer : ListViewRenderer
    {
        public NonScrollableListViewRenderer(Context context) : base(context) { }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.ListView> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                Control.VerticalScrollBarEnabled = false;
            }
        }
    }
}

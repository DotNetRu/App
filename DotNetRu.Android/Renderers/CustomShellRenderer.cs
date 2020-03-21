using Android.Content;
using DotNetRu.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Shell), typeof(CustomShellRenderer))]
namespace DotNetRu.Droid.Renderers
{
    public class CustomShellRenderer : ShellRenderer
    {
        public CustomShellRenderer(Context context) : base(context) { }

        protected override IShellItemRenderer CreateShellItemRenderer(ShellItem shellItem)
        {
            return new SelectedShellItemRenderer(this);
        }
    }
}

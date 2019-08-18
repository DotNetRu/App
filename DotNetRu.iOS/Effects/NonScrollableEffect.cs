using Conference.iOS.Effects;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ResolutionGroupName("DotNetRu")]
[assembly: ExportEffect(typeof(NonScrollableEffect), nameof(NonScrollableEffect))]
namespace Conference.iOS.Effects
{
    public class NonScrollableEffect : PlatformEffect
    {
        private UITableView NativeList => Control as UITableView;

        protected override void OnAttached()
        {
            if (NativeList != null)
            {
                NativeList.ScrollEnabled = false;
            }
        }

        protected override void OnDetached()
        {
        }
    }
}

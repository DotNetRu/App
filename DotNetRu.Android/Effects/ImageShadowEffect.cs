using System;
using System.Linq;
using DotNetRu.Clients.UI.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XamarinEvolve.Droid.Effects;

[assembly: ExportEffect(typeof(ImageShadowEffect), "ImageShadowEffect")]

namespace XamarinEvolve.Droid.Effects
{
    public class ImageShadowEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            try
            {
                if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Lollipop)
                {
                    var control = this.Control as Android.Widget.ImageView;
                    var effect = (ShadowEffect) this.Element.Effects.FirstOrDefault(e => e is ShadowEffect);

                    control.Elevation = Math.Max(effect.DistanceX, effect.DistanceY);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Cannot set property on attached control. Error: ", ex.Message);
            }
        }

        protected override void OnDetached()
        {
        }
    }
}
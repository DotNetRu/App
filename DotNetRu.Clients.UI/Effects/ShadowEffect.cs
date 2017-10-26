namespace XamarinEvolve.Clients.UI.Effects
{
    using Xamarin.Forms;

    public class ShadowEffect : RoutingEffect
    {
        public float Radius { get; set; }

        public Color Color { get; set; }

        public float DistanceX { get; set; }

        public float DistanceY { get; set; }

        public ShadowEffect() : base("Xpirit.ImageShadowEffect")
        {
        }
    }
}

namespace DotNetRu.Clients.UI.Controls
{
    using Xamarin.Forms;

    public class ParallaxScrollView : AlwaysScrollView
    {
        public static readonly BindableProperty ParallaxViewProperty = BindableProperty.Create(
            nameof(ParallaxView),
            typeof(View),
            typeof(ParallaxScrollView));

        private double height;

        public ParallaxScrollView()
        {
            this.Scrolled += (sender, e) => this.Parallax();
        }

        public View ParallaxView
        {
            get => (View)this.GetValue(ParallaxViewProperty);
            set => this.SetValue(ParallaxViewProperty, value);
        }

        public void Parallax()
        {
            if (this.ParallaxView == null || Device.RuntimePlatform == Device.UWP)
            {
                return;
            }

            if (this.height <= 0)
            {
                this.height = this.ParallaxView.Height;
            }

            var y = -(int)((float)this.ScrollY / 2.5f);
            if (y < 0)
            {
                // Move the Image's Y coordinate a fraction of the ScrollView's Y position
                this.ParallaxView.Scale = 1;
                this.ParallaxView.TranslationY = y;
            }
            else if (Device.RuntimePlatform == Device.iOS)
            {
                // Calculate a scale that equalizes the height vs scroll
                double newHeight = this.height + (this.ScrollY * -1);
                this.ParallaxView.Scale = newHeight / this.height;
                this.ParallaxView.TranslationY = -(this.ScrollY / 2);
            }
            else
            {
                this.ParallaxView.Scale = 1;
                this.ParallaxView.TranslationY = 0;
            }
        }
    }
}
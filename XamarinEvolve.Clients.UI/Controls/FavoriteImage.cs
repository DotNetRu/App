
using Xamarin.Forms;
using System.Threading.Tasks;

namespace XamarinEvolve.Clients.UI
{
    public class FavoriteImage : Image
    {
        bool addedAnimation;
		bool isAnimating;

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            if (addedAnimation || GestureRecognizers.Count == 0)
                return;

            addedAnimation = true;

            var tapGesture = GestureRecognizers[0] as TapGestureRecognizer;
            if (tapGesture == null)
                return;

            tapGesture.Tapped += (sender, e) => 
            {
                Device.BeginInvokeOnMainThread (() => Grow ());
            };

        }

        /// <summary>
        /// Play animation to grow and shrink
        /// </summary>
        public void Grow()
        {
			if (isAnimating)
				return;

			isAnimating = true;

			try
			{
				this.ScaleTo(1.4, 75).ContinueWith((t) =>
			  {
				  try
				  {
					  this.ScaleTo(1.0, 75);
				  }
				  catch
				  {
				  }
			  },
				scheduler: TaskScheduler.FromCurrentSynchronizationContext());
			}
			catch
			{
			}
			finally
			{
				isAnimating = false;
			}
        }
    }
}


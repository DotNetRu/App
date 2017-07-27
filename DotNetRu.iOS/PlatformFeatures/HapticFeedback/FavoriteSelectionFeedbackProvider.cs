using UIKit;
using Xamarin.Forms;
using XamarinEvolve.Clients.Portable;
using XamarinEvolve.DataObjects;
using XamarinEvolve.iOS;

[assembly:Dependency(typeof(FavoriteSelectionFeedbackProvider))]

namespace XamarinEvolve.iOS
{
	public class FavoriteSelectionFeedbackProvider : IPlatformActionWrapper<Session>
	{
		UINotificationFeedbackGenerator _feedback;

		public void Before(Session contextEntity)
		{
			if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
			{
				_feedback = new UINotificationFeedbackGenerator();
				_feedback.Prepare();
			}
		}

		public void After(Session contextEntity)
		{
            _feedback?.NotificationOccurred(UINotificationFeedbackType.Success);
		}
	}
}

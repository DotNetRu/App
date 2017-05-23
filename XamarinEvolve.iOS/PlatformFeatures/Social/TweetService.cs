using System.Threading.Tasks;
using Social;
using UIKit;
using Xamarin.Forms;
using XamarinEvolve.Clients.Portable;
using XamarinEvolve.iOS.PlatformFeatures.Social;
using XamarinEvolve.Utils;

[assembly:Dependency(typeof(TweetService))]

namespace XamarinEvolve.iOS.PlatformFeatures.Social
{
	public class TweetService : ITweetService
	{
		public async Task InitiateConferenceTweet()
		{
			var slComposer = SLComposeViewController.FromService(SLServiceType.Twitter);
			if (slComposer == null)
			{
				var alertController = UIAlertController.Create(
								"Unavailable",
								"Twitter is not available, please sign in on your device's settings screen.",
								UIAlertControllerStyle.Alert);
				alertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
				await UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewControllerAsync(alertController, true);
			}
			else
			{
				slComposer.SetInitialText(EventInfo.HashTag + " ");
				if (slComposer.EditButtonItem != null)
				{
					slComposer.EditButtonItem.TintColor = AppDelegate.PrimaryColor;
				}
				slComposer.CompletionHandler += (result) =>
				{
					UIApplication.SharedApplication.InvokeOnMainThread(() => UIApplication.SharedApplication.KeyWindow.RootViewController.DismissViewController(true, null));
				};

				await UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewControllerAsync(slComposer, true);
			}
		}
	}
}

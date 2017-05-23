using System;
using Foundation;
using UIKit;
using Xamarin.Forms;
using XamarinEvolve.Clients.Portable;
using XamarinEvolve.iOS;

[assembly: Dependency(typeof(LaunchFacebook))]

namespace XamarinEvolve.iOS
{
	public class LaunchFacebook: ILaunchFacebook
	{
		public bool OpenUserName(string username)
		{
			try
			{
				var facebookUrl = NSUrl.FromString($"fb://profile?id={username}");
				if (UIApplication.SharedApplication.OpenUrl(facebookUrl))
					return true;
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine("Unable to launch url" + ex);
			}
			return false;
		}
	}
}


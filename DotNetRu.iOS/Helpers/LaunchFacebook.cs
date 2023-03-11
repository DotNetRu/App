using System;
using DotNetRu.Clients.Portable.Interfaces;
using DotNetRu.iOS.Helpers;
using Foundation;
using UIKit;
using Microsoft.Maui;

[assembly: Dependency(typeof(LaunchFacebook))]

namespace DotNetRu.iOS.Helpers
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


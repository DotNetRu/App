using System;

using Foundation;

using UIKit;

using Xamarin.Forms;

using XamarinEvolve.Clients.Portable;
using XamarinEvolve.iOS;

[assembly:Dependency(typeof(LaunchTwitter))]
namespace XamarinEvolve.iOS
{
    public class LaunchTwitter : ILaunchTwitter
    {
        #region ILaunchTwitter implementation

        public bool OpenUserName(string username)
        {
            try
            {
				var twitterUrl = NSUrl.FromString($"twitter://user?screen_name={username}");
				if (UIApplication.SharedApplication.OpenUrl(twitterUrl))
					return true;
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Unable to launch url" + ex);
            }

            try
            {
				var tweetbotUrl = NSUrl.FromString($"tweetbot:///user_profile/{username}");
				if (UIApplication.SharedApplication.OpenUrl(tweetbotUrl))
					return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Unable to launch url " + ex);
            }

            return false;
        }

        public bool OpenStatus(string statusId)
        {
            
            try
            {
                if(UIApplication.SharedApplication.OpenUrl(NSUrl.FromString($"twitter://status?id={statusId}")))
                    return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Unable to launch url " + ex);
            }

            try
            {
                if(UIApplication.SharedApplication.OpenUrl(NSUrl.FromString($"tweetbot:///status/{statusId}")))
                    return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Unable to launch url " + ex);
            }

            return false;
        }

        #endregion


    }
}


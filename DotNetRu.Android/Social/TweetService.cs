using System.Threading.Tasks;
using Plugin.Share;
using Xamarin.Forms;
using XamarinEvolve.Clients.Portable;
using XamarinEvolve.Utils;
using XamarinEvolve.Droid;

[assembly: Dependency(typeof(TweetService))]

namespace XamarinEvolve.Droid
{
	public class TweetService : ITweetService
	{
		public async Task InitiateConferenceTweet()
		{
			await CrossShare.Current.Share(EventInfo.HashTag + " ");
		}
	}
}

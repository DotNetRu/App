using Plugin.Share;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamarinEvolve.Clients.Portable;
using XamarinEvolve.Utils;
using XamarinEvolve.UWP.Social;

[assembly:Dependency(typeof(TweetService))]

namespace XamarinEvolve.UWP.Social
{
    public class TweetService : ITweetService
    {
        public async Task InitiateConferenceTweet()
        {
            await CrossShare.Current.Share(EventInfo.HashTag + " ", "Share");
        }
    }
}

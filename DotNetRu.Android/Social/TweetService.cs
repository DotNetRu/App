using System.Threading.Tasks;
using Plugin.Share;
using Xamarin.Forms;
using XamarinEvolve.Clients.Portable;
using XamarinEvolve.Droid;

[assembly: Dependency(typeof(TweetService))]

namespace XamarinEvolve.Droid
{
    using Plugin.Share.Abstractions;

    using XamarinEvolve.Utils.Helpers;

    public class TweetService : ITweetService
    {
        public async Task InitiateConferenceTweet()
        {
            await CrossShare.Current.Share(new ShareMessage { Text = EventInfo.HashTag + " " });
        }
    }
}

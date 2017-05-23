

using System;

namespace XamarinEvolve.Clients.Portable
{
    public class DeepLinkPage
    {
        public AppPage Page { get; set; }
        public string Id { get; set;}
    }

	public enum AppPage
	{
		Feed,
		Sessions,
		Speakers,
		Events,
		MiniHacks,
		Sponsors,
		Venue,
		FloorMap,
		ConferenceInfo,
		Settings,
		Session,
		Speaker,
		Sponsor,
		Login,
		Event,
		Notification,
		TweetImage,
		WiFi,
		CodeOfConduct,
		Filter,
		Information,
		Tweet,
		Evals,
		SyncMobileToWeb,
		SyncWebToMobile,
		Feedback,
		MiniHack,
		ConferenceFeedback,
	}
}



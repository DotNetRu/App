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
		Sponsors,
		Settings,
		Session,
		Speaker,
		Sponsor,
		Event,
		Notification,
		TweetImage,
		Filter,
		Information,
		Tweet,
		SyncMobileToWeb,
		SyncWebToMobile,
		Feedback,
		ConferenceFeedback,
	}
}



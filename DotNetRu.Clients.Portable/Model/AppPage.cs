namespace XamarinEvolve.Clients.Portable
{
    public class DeepLinkPage
    {
        public AppPage Page { get; set; }

        public string Id { get; set; }
    }

    public enum AppPage
    {
        Feed,

        Meetup,

        Speakers,

        Meetups,

        Friends,

        Settings,

        Talk,

        Speaker,

        Friend,

        Event,

        Notification,

        TweetImage,

        Filter,

        Information,

        Tweet,

        Feedback,

        ConferenceFeedback,
    }
}



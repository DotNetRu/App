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

        Sessions,

        Speakers,

        Events,

        Friends,

        Settings,

        Session,

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



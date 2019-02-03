namespace DotNetRu.Clients.Portable.Model
{
    public class DeepLinkPage
    {
        public AppPage Page { get; set; }

        public string Id { get; set; }
    }

    public enum AppPage
    {
        News,

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

        SpeakerFace
    }
}



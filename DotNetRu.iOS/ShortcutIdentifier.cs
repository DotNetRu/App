namespace DotNetRu.iOS
{
    using DotNetRu.Utils.Helpers;

    public partial class AppDelegate
    {
        public static class ShortcutIdentifier
        {
            public const string Tweet = AboutThisApp.PackageName + ".tweet";

            public const string Announcements = AboutThisApp.PackageName + ".announcements";

            public const string Events = AboutThisApp.PackageName + ".events";
        }
    }
}


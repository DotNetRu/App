using System;

namespace DotNetRu.Utils.Helpers
{
    public static class AboutThisApp
    {
        public const string AppLinkProtocol = "dotnetru";

        public const string PackageName = "com.dotnetru.app";

        public const string AppName = "DotNetRu";

        public const string CompanyName = "DotNetRu";

        public const string Developer = "DotNetRu Team";

        public const string DotNetRuLink = "http://dotnet.ru/";

        public static readonly Uri IssueTracker = new Uri("https://github.com/DotNetRu/App/issues");

        // TODO fix, should be link to oss licenses
        public const string OpenSourceNoticeUrl = "https://github.com/DotNetRu/App/blob/master/LICENSE.md";

        // TODO: use the domain name of the site you want to integrate AppLinks with
        public const string AppLinksBaseDomain = "TODO";

        public const string SessionsSiteSubdirectory = "Sessions";

        public const string SpeakersSiteSubdirectory = "Speakers";

        public const string Copyright = "Copyright 2018 - DotNetRu";
    }
}


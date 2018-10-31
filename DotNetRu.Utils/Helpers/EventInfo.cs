namespace DotNetRu.Utils.Helpers
{
    using System;

    public static class EventInfo
    {
        /// <summary>
        /// The time zone name. See https://EN.wikipedia.org/wiki/List_of_TZ_database_time_zones
        /// </summary>
        public const string TimeZoneName = "Europe/Moscow";

        public const string HashTag = "#DotNetRu";

        public static readonly DateTime StartOfConference = new DateTime(2016, 10, 04, 6, 0, 0, DateTimeKind.Utc);

        public static readonly DateTime EndOfConference = new DateTime(2016, 10, 05, 15, 30, 0, DateTimeKind.Utc);

        public static string TicketUrl { get; set; }
    }
}
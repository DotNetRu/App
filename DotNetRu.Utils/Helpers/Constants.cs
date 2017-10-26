namespace XamarinEvolve.Utils.Helpers
{
    using System;

    /// <summary>
    /// The event info.
    /// </summary>
    public static class EventInfo
    {
        /// <summary>
        /// The address 1.
        /// </summary>
        public const string Address1 = "Saint Petersburg";

        /// <summary>
        /// The address 2.
        /// </summary>
        public const string Address2 = "Saint Petersburg";

        /// <summary>
        /// The latitude.
        /// </summary>
        public const double Latitude = 52.340681d;

        /// <summary>
        /// The longitude.
        /// </summary>
        public const double Longitude = 4.889541d;

        /// <summary>
        /// The time zone name. See https://EN.wikipedia.org/wiki/List_of_TZ_database_time_zones
        /// </summary>
        public const string TimeZoneName = "Europe/Moscow";

        /// <summary>
        /// The hash tag.
        /// </summary>
        public const string HashTag = "#DotNetRu";

        /// <summary>
        /// The ticket url.
        /// </summary>
        public const string TicketUrl = "https://todo";

        /// <summary>
        /// The start of conference.
        /// </summary>
        public static readonly DateTime StartOfConference = new DateTime(2016, 10, 04, 6, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// The end of conference.
        /// </summary>
        public static readonly DateTime EndOfConference = new DateTime(2016, 10, 05, 15, 30, 0, DateTimeKind.Utc);
    }
}
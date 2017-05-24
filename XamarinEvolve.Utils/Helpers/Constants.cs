using System;

namespace XamarinEvolve.Utils
{
	public static class EventInfo
	{
		public const string EventShortName = "TechDays16";
		public const string EventName = "TechDays";
		public const string Address1 = "Europaplein 22";
		public const string Address2 = "1078 GZ Amsterdam";
		public const string VenueName = "RAI Amsterdam";
		public const string VenuePhoneNumber = "+31 20 549 1212";
		public const double Latitude = 52.340681d;
		public const double Longitude = 4.889541d;
		public const string TimeZoneName = "Europe/Amsterdam"; //https://en.wikipedia.org/wiki/List_of_tz_database_time_zones
		public const string HashTag = "#TechDaysNL";

		public const string TicketUrl = "https://techdays.msnlevents.net/techdays2016";
		public static readonly DateTime StartOfConference = new DateTime(2016, 10, 04, 6, 0, 0, DateTimeKind.Utc);
		public static readonly DateTime EndOfConference = new DateTime(2016, 10, 05, 15, 30, 0, DateTimeKind.Utc);
	}

	public static class AboutThisApp
	{
		public const string AppLinkProtocol = "techdays";
		public const string PackageName = "com.xpirit.techdays";
		public const string AppName = "TechDays16";
		public const string CompanyName = "Xpirit";
		public const string Developer = "Xpirit";
		public const string DeveloperWebsite = "https://www.xpirit.com/mobile";
		public const string OpenSourceUrl = "http://tiny.cc/app-evolve";
		public const string TermsOfUseUrl = "http://go.microsoft.com/fwlink/?linkid=206977";
		public const string PrivacyPolicyUrl = "http://go.microsoft.com/fwlink/?LinkId=521839";
		public const string OpenSourceNoticeUrl = "http://tiny.cc/app-evolve-osn";
		public const string EventRegistrationPage = "https://techdays.msnlevents.net/techdays2016";
		public const string CdnUrl = "https://s3.eu-central-1.amazonaws.com/xpirit-techdays2016/"; // TODO: set up your own CDN for static content
		public const string AppLinksBaseDomain = "www.techdays.nl"; // TODO: use the domain name of the site you want to integrate AppLinks with
		public const string SessionsSiteSubdirectory = "Sessies";
		public const string SpeakersSiteSubdirectory = "Sprekers";
		public const string SponsorsSiteSubdirectory = "Sponsors";
		public const string Copyright = "Copyright 2016 - TechDays";
		public const string CodeOfConductPageTitle = "Permission to be filmed";

		public const string Credits = "The TechDays 2016 mobile apps were handcrafted by Xpirit, based on the great work done by Xamarin.\n\n" +
			"Xpirit Team:\n" +
			"Roy Cornelissen\n" +
			"Marcel de Vries\n" +
			"Geert van der Cruijsen\n\n" +
			"Team TechDays NL\n\n" +
			"Many thanks to the original Xamarin Team:\n" +
			"James Montemagno\n" +
			"Pierce Boggan\n" +
		   "\n" +
			"...and of course you! <3";
	}

	public static class PublicationSettings
	{
		public const uint iTunesAppId = 1111111111; // Your iTunes app ID
	}

	public static class ApiKeys
	{
		public const string ApiUrl = "https://techdays-2016.azurewebsites.net";
		public const string HockeyAppiOS = ""; // HockeyApp App ID for iOS app
		public const string HockeyAppAndroid = ""; // HockeyApp App ID for Android app
		public const string HockeyAppUWP = ""; // HockeyApp App ID for UWP app

		public const string GoogleAnalyticsTrackingId = "UA-1111111-1"; // Your Google Analytics tracking ID

		public const string AzureHubName = "Techdays2016";
		public const string AzureServiceBusUrl = "sb://techdays-2016.servicebus.windows.net/";
		public const string AzureKey = ""; // TODO: take from your Azure portal
		public const string GoogleSenderId = ""; // TODO: take from your Google Developer Console

		public const string BingMapsUWPKey = "";
	}

	public static class MessageKeys
	{
		public const string NavigateToEvent = "navigate_event";
		public const string NavigateToSession = "navigate_session";
		public const string NavigateToSpeaker = "navigate_speaker";
		public const string NavigateToSponsor = "navigate_sponsor";
		public const string NavigateToImage = "navigate_image";
		public const string NavigateLogin = "navigate_login";
		public const string NavigateToSyncMobileToWebViewModel = "navigate_syncmobiletoweb";
		public const string NavigateToSyncWebToMobileViewModel = "navigate_syncwebtomobile";
		public const string NavigateToConferenceFeedback = "navigate_conferencefeedback";
		public const string SessionFavoriteToggled = "session_favorite_toggled";
		public const string Error = "error";
		public const string Connection = "connection";
		public const string LoggedIn = "loggedin";
		public const string Message = "message";
		public const string Question = "question";
		public const string Choice = "choice";
	}
}


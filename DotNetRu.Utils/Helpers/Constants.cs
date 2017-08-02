using System;

namespace XamarinEvolve.Utils
{
	public static class EventInfo
	{
		public const string EventShortName = "DotNetRu";
		public const string EventName = "DotNetRu";
		public const string Address1 = "Saint Petersburg";
		public const string Address2 = "Saint Petersburg";
		public const double Latitude = 52.340681d;
		public const double Longitude = 4.889541d;
		public const string TimeZoneName = "Moscow"; //https://en.wikipedia.org/wiki/List_of_tz_database_time_zones
		public const string HashTag = "#DotNetRu";

		public const string TicketUrl = "https://todo";
		public static readonly DateTime StartOfConference = new DateTime(2016, 10, 04, 6, 0, 0, DateTimeKind.Utc);
		public static readonly DateTime EndOfConference = new DateTime(2016, 10, 05, 15, 30, 0, DateTimeKind.Utc);
	}

	public static class AboutThisApp
	{
		public const string AppLinkProtocol = "dotnetru";
		public const string PackageName = "com.dotnetru.app";
		public const string AppName = "DotNetRu App";
		public const string CompanyName = "DotNetRu";
		public const string Developer = "DotNetRu";
		public const string DeveloperWebsite = "https://www.xpirit.com/mobile";
		public const string OpenSourceUrl = "http://tiny.cc/app-evolve";
		public const string TermsOfUseUrl = "http://go.microsoft.com/fwlink/?linkid=206977";
		public const string PrivacyPolicyUrl = "http://go.microsoft.com/fwlink/?LinkId=521839";
		public const string OpenSourceNoticeUrl = "http://tiny.cc/app-evolve-osn";
		public const string EventRegistrationPage = "TODO";
	  // TODO: use the domain name of the site you want to integrate AppLinks with
    public const string AppLinksBaseDomain = "TODO";
		public const string SessionsSiteSubdirectory = "Sessies";
		public const string SpeakersSiteSubdirectory = "Sprekers";
		public const string SponsorsSiteSubdirectory = "Sponsors";
		public const string Copyright = "Copyright 2017 - DotNetRu";

		public const string Credits = "The DotNetRu mobile app were handcrafted by DotNetRu, based on the great work done by Xamarin.\n\n" +
			"DotNetRu Team:\n" +
			"Anatoly Kulakov\n" +
			"Pavel Fedotovsky\n" +
      "Yury Belousov\n" +
      "Sergey Polezhaev\n\n" +
			"Many thanks to the original Xamarin Team:\n" +
			"James Montemagno\n" +
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


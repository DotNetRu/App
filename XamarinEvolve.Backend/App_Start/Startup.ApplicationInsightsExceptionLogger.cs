using Microsoft.ApplicationInsights;
using System.Web.Http.ExceptionHandling;

namespace XamarinEvolve.Backend.App_Start
{
    public class ApplicationInsightsExceptionLogger: ExceptionLogger
	{
        private static readonly TelemetryClient TelemetryClient = new TelemetryClient();

        public override void Log(ExceptionLoggerContext context)
        {
            if (context != null && context.Exception != null)
            {
                TelemetryClient.TrackException(context.Exception);
            }
            base.Log(context);
        }
    }
}
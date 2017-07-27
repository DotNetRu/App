using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;
using Microsoft.ApplicationInsights.Web.Implementation;
using Owin;
using Microsoft.Owin;
using XamarinEvolve.Backend.App_Start;

namespace XamarinEvolve.Backend.App_Start
{
    //azure mobile app services block module loading
    // this class generates events like the http handler will, so we can still picku the events
    public class ApplicationInsightsMobileAppRequestHandler : OwinMiddleware
    {
        private readonly TelemetryClient telemetryClient;

        public ApplicationInsightsMobileAppRequestHandler(OwinMiddleware next) : base(next)
        {
            try
            {
                // The call initializes TelemetryConfiguration that will create and Intialize modules
                TelemetryConfiguration configuration = TelemetryConfiguration.Active;
                telemetryClient = new TelemetryClient(configuration);
            }
            catch (Exception exc)
            {
                Trace.WriteLine("Error initializing Handler");
                Trace.WriteLine(exc.Message);
            }
        }



        public override async Task Invoke(IOwinContext context)
        {
            var operation = telemetryClient.StartOperation<RequestTelemetry>(context.Request.Path.Value);

            try
            {
                var requestTelemetry = operation.Telemetry;

                await this.Next.Invoke(context);

                requestTelemetry.HttpMethod = context.Request.Method;
                requestTelemetry.Url = context.Request.Uri;
                requestTelemetry.ResponseCode = context.Response.StatusCode.ToString();
                requestTelemetry.Success = context.Response.StatusCode >= 200 && context.Response.StatusCode < 300;
            }
            catch (Exception exc)
            {
                var telemetry = new ExceptionTelemetry(exc);
                telemetry.HandledAt = ExceptionHandledAt.Unhandled;
                telemetryClient.TrackException(telemetry);
            }
            finally
            {
                telemetryClient.StopOperation(operation);
            }
        }
    }



    public static class AppBuilderExtensions
    {
        public static IAppBuilder UseMobileAppRequestHandler(this IAppBuilder app)
        {
            return app.Use<ApplicationInsightsMobileAppRequestHandler>();
        }
    }
}
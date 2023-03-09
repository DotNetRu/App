using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace DotNetRu.AzureService.Logging
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public RequestResponseLoggingMiddleware(RequestDelegate next,
            ILogger<RequestResponseLoggingMiddleware> logger)
        {
            _logger = logger;
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var request = await FormatRequest(context.Request);

            _logger.LogInformation($"Request:{Environment.NewLine}" +
               $"Host: {context.Request.Host} " +
               $"Path: {context.Request.Path} " +
               $"QueryString: {context.Request.QueryString} " +
               $"Request Body: {request}");

            var originalBodyStream = context.Response.Body;

            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;

                await _next(context);

                var response = await FormatResponse(context.Response);

                _logger.LogInformation($"Response Information:{Environment.NewLine}" +
                                           $"Host: {context.Request.Host} " +
                                           $"Path: {context.Request.Path} " +
                                           $"QueryString: {context.Request.QueryString} " +
                                           $"Response Body: {response}");

                await responseBody.CopyToAsync(originalBodyStream);
            }
        }

        private static async Task<string> FormatRequest(HttpRequest request)
        {
            var body = request.Body;

            request.EnableBuffering();

            var buffer = new byte[Convert.ToInt32(request.ContentLength)];

            await request.Body.ReadAsync(buffer, 0, buffer.Length);

            var bodyAsText = Encoding.UTF8.GetString(buffer);

            request.Body = body;

            return $"{request.Scheme} {request.Host}{request.Path} {request.QueryString} {bodyAsText}";
        }

        private static async Task<string> FormatResponse(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);

            string text = await new StreamReader(response.Body).ReadToEndAsync();

            response.Body.Seek(0, SeekOrigin.Begin);

            return $"{response.StatusCode}: {text}";
        }
    }

}

using Microsoft.Extensions.Logging;

namespace DotNetRu.Azure
{
    /// <summary>
    /// Shared logger
    /// </summary>
    internal static class ApplicationLogging
    {
        internal static ILoggerFactory LoggerFactory { get; set; }// = new LoggerFactory();
        internal static ILogger CreateLogger<T>() => LoggerFactory.CreateLogger<T>();
        internal static ILogger CreateLogger(string categoryName) => LoggerFactory.CreateLogger(categoryName);

    }
}

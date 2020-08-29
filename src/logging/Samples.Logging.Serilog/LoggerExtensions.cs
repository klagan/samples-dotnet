namespace Samples.Logging.Serilog
{
    using System;
    using Microsoft.Extensions.Logging;

    public static class LoggerExtensions
    {
        private static class EventIds
        {
            public static readonly EventId RequestStart = new EventId(100, "Request Start");
            public static readonly EventId RequestEnd = new EventId(101, "Request End");
        }

        private static readonly Func<ILogger, string, IDisposable> _requestScope =
           LoggerMessage.DefineScope<string>(
               "CorrelationId: [{correlationid}]");

        private static readonly Action<ILogger, string, Exception> _logRequest =
           LoggerMessage.Define<string>(
               LogLevel.Information,
               EventIds.RequestStart,
               "Start processing HTTP request: {message}]");

        public static IDisposable SetRequestScope(this ILogger logger, string correlationId)
        {
            return _requestScope(logger, correlationId);
        }

        public static void LogRequest(this ILogger logger, string message)
        {
            _logRequest(logger, message, null);
        }
    }
}

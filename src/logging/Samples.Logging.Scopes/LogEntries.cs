using System;
using Microsoft.Extensions.Logging;

namespace Samples.Logging.Scopes
{
    public static class LogEntries
    {
        static readonly Action<ILogger, string, Exception> StaticEntry = LoggerMessage.Define<string>(LogLevel.Information, new EventId(1, "My Name"), "My message : {stuff}");

        static readonly Action<ILogger, string, string, int, Exception> ExceptionEntry = LoggerMessage.Define<string, string, int>(LogLevel.Error, 0, "string 1: {string1}, string 2: {string2}, string 3: {string3}");

        public static void StaticLogger(this ILogger logger)
        {
            StaticEntry(logger, "kam", null);
        }

        public static void ExceptionLogger(this ILogger logger, string param1, string somethingElse, int number, Exception e)
        {
            ExceptionEntry(logger, param1, somethingElse, number, e);
        }
    }
}

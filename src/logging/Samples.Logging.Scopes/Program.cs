using System;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Samples.Logging.Scopes
{
    class Program
    {
        static InMemoryChannel _channel;

        static void Main(string[] args)
        {

            var services = ConfigureServices();

            // disposing the serviceProvider allows the messages to be flushed from the buffer
            using (var serviceProvider = services.BuildServiceProvider())
            {

                var logger = serviceProvider.GetRequiredService<ILogger<Program>>();


                using(logger.BeginScope("Kam with channel"))
                {
                    LogTestMessages(logger);

                    logger.StaticLogger();

                    logger.ExceptionLogger("first", "second", 919, new Exception("kam exception"));
                }

                LogTestMessages(logger);

                logger.StaticLogger();

                logger.ExceptionLogger("first", "second", 919, new Exception("kam exception"));
            }

            //_channel.Flush();

            var test = services.BuildServiceProvider().GetRequiredService<Test>();

            test.Log("Kam");
            test.Log();

        }

        static IServiceCollection ConfigureServices()
        {
            // set up the logger

            var services = new ServiceCollection();

            //_channel = new InMemoryChannel();
            //services.Configure<TelemetryConfiguration>(
            //    (config) =>
            //    {
            //        config.TelemetryChannel = _channel;
            //    }
            //);

            //services.AddApplicationInsightsTelemetry("7bba006c-252f-456d-b3b9-ad7c27cffc56");

            //services.AddApplicationInsightsTelemetry(options =>
            //{
            //    options.InstrumentationKey = "7bba006c-252f-456d-b3b9-ad7c27cffc56";
            //    options.DeveloperMode = false;
            //});

            services.AddLogging(builder =>
            {
                builder.AddConsole(options =>
                {
                    options.IncludeScopes = true;
                });

                builder.AddApplicationInsights("7bba006c-252f-456d-b3b9-ad7c27cffc56", options =>
                {
                    options.IncludeScopes = true;
                    options.TrackExceptionsAsExceptionTelemetry = true;
                });

                //builder.AddApplicationInsights("7bba006c-252f-456d-b3b9-ad7c27cffc56");

                //builder.AddFilter<Microsoft.Extensions.Logging.ApplicationInsights.ApplicationInsightsLoggerProvider>
                //                ("", LogLevel.Information);
            });

            services.AddTransient<Test>();

            return services;
        }

        static void LogTestMessages(ILogger logger)
        {
            // test messages

            logger.LogInformation("information message");

            logger.LogWarning("warning message");

            logger.LogError("error message");

            logger.LogCritical("critical message");
        }
    }
}

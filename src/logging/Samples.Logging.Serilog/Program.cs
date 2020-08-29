namespace Samples.Logging.Serilog
{
    using System;
    using global::Serilog;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    // https://nblumhardt.com/2016/11/ilogger-beginscope/
    // https://www.initpals.com/net-core/scoped-logging-using-microsoft-logger-with-serilog-in-net-core-application/
    // https://www.stevejgordon.co.uk/httpclientfactory-asp-net-core-logging
    // https://weblog.west-wind.com/posts/2018/Dec/31/Dont-let-ASPNET-Core-Default-Console-Logging-Slow-your-App-down
    // https://github.com/serilog/serilog-extensions-logging-file
    // https://mitchelsellers.com/blogs/2017/10/09/real-world-aspnet-core-logging-configuration

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            // declaratively configure the serilog global logger
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();

            var services = new ServiceCollection();

            services.AddLogging(builder => builder.AddSerilog(dispose: true));

            var logger = services.BuildServiceProvider().GetRequiredService<ILogger<Program>>();

            TestLog(logger);
        }

        static void TestLog(Microsoft.Extensions.Logging.ILogger logger)
        {
            using (logger.SetRequestScope("my correlation id"))
            {
                logger.LogRequest("my custom log message");
            }
        }
    }
}

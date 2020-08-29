namespace Samples.Host
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    class Program
    {
        // https://github.com/stevejgordon/GenericHostExample/blob/master/GenericHostExample/Program.cs

        static async Task Main()
        {
            var host = new HostBuilder()
                    .ConfigureServices((hostContext, services) =>
                    {
                        services.AddOptions();

                        services.Configure<HostOptions>(option =>
                        {
                            option.ShutdownTimeout = TimeSpan.FromSeconds(5);
                        });

                        services.AddHostedService<MyHostedService>();
                    })
                    .ConfigureLogging((hostContext, logging) =>
                    {
                        logging.AddConsole();
                    })
                    .Build();

            await host.RunAsync();
        }
    }
}
namespace Samples.Host
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;

    public class MyHostedService : MyBaseHostedService
    {
        private readonly ILogger<MyHostedService> _logger;

        public MyHostedService(ILogger<MyHostedService> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                // do stuff here
                _logger.LogInformation($"my background hosted service running => [{DateTime.Now}]");

                // little delay
                await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
            }
        }
    }
}
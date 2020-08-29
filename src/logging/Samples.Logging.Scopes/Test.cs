using Microsoft.Extensions.Logging;

namespace Samples.Logging.Scopes
{
    public class Test
    {

        private ILogger _logger;

        public Test(ILogger<Test> logger)
        {
            _logger = logger;
        }

        public void Log(string message = "no message sent")
        {
            _logger.LogError(message);
        }
    }
}
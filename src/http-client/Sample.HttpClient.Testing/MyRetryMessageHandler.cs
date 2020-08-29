namespace Sample.HttpClient.Testing
{
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    public class RetryMessageHandler : DelegatingHandler
    {
        readonly int _retryCount;
        
        public RetryMessageHandler(int retryCount)
            : this(new HttpClientHandler(), retryCount) { }
        
        public RetryMessageHandler(HttpMessageHandler innerHandler, int retryCount)
            : base(innerHandler)
        {
            _retryCount = retryCount;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpResponseMessage response = null;
            
            for (var i = 0; i<=_retryCount; i++)
            {
                response = await base.SendAsync(request, cancellationToken);
                
                if (response.IsSuccessStatusCode)
                {
                    return response;
                }
            }
            
            return response;
        }
    }
}
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sample.HttpClient.Testing
{
    using HttpClient = System.Net.Http.HttpClient;

    class Program
    {
        static void Main(string[] args)
        {
            var myResponse = new HttpResponseMessage(HttpStatusCode.OK)
                {Content = new StringContent("{name:kam}", Encoding.UTF8, "application/json")};

            var myHandler = new MyHttpMessageHandler(myResponse);

            var client = new HttpClient(myHandler);

            var response = client.GetAsync("https://kam.lagan.me");
            var content = response.Result.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            Console.WriteLine($"{response.Result.StatusCode}\n{response.Result.Headers}\n{content}");

            response = client.PostAsync("https://kam.lagan.me", new StringContent("{name:test"));
            content = response.Result.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            Console.WriteLine($"{response.Result.StatusCode}\n{response.Result.Headers}\n{content}");

//            HttpClient httpClient = HttpClientFactory.CreatePipeline(new HttpClientHandler(),
//                new [] { new MyRetryMessageHandler(3) });
//
//            var response = await httpClient.SendAsync(request);

            Console.WriteLine("***End***");

            Console.ReadLine();
        }
    }
}

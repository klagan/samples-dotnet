using System;

namespace Sample.Channels
{
    using System.Threading.Channels;
    using System.Threading.Tasks;

    // https://channel9.msdn.com/Shows/On-NET/Working-with-Channels-in-NET
    
    class Program
    {
        static async Task Main()
        {
            // this magic line creates a channel (think of a queue)
            var channel = Channel.CreateUnbounded<int>();

            // this section runs on a background thread
            _ = Task.Run(async delegate
            {
                for (var i = 0;; i++)
                {
                    await Task.Delay(1000);
                    
                    // you can write to the channel
                    channel.Writer.TryWrite(i);
                }
            });

            // this runs on the main thread
            while (true)
            {
                // and will be notified of something being on the channel so react to it
                Console.WriteLine(await channel.Reader.ReadAsync());
            }
        }
    }
}
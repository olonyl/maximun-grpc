using Calculator;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace client
{
    class Program
    {
        const string Target = "127.0.0.1:50050";
        static async Task Main(string[] args)
        {
            Channel channel = new Channel(Target, ChannelCredentials.Insecure);


            await channel.ConnectAsync().ContinueWith((task) =>
            {
                if (task.Status == TaskStatus.RanToCompletion)
                {
                    Console.WriteLine("Successfully connected!");
                }
            });
            var client = new CalculatorService.CalculatorServiceClient(channel);
            var stream = client.FindMaximun();

            List<int> numbers = new List<int> { 1, 5, 3, 6, 2, 20 };
            var responseRenderTask = Task.Run(async () =>
            {
                while (await stream.ResponseStream.MoveNext())
                {
                    Console.WriteLine($"Received: " + stream.ResponseStream.Current.Result);
                }
            });

            foreach (var number in numbers)
            {
                Console.WriteLine($"Sending {number}");
                await stream.RequestStream.WriteAsync(new FindMaximunRequest { Value = number });
            }
            await stream.RequestStream.CompleteAsync();
            await responseRenderTask;



            channel.ShutdownAsync().Wait();
            Console.ReadKey();
        }
    }
}

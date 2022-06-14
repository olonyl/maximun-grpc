using Calculator;
using Grpc.Core;
using System;
using System.IO;

namespace server
{
    class Program
    {
        const int Port = 50050;
        static void Main(string[] args)
        {
            Server server = null;

            try
            {
                server = new Server
                {
                    Services = { CalculatorService.BindService(new CalculatorServiceImpl()) },
                    Ports = { new ServerPort("localhost", Port, ServerCredentials.Insecure) }
                };

                server.Start();
                Console.WriteLine($"Server started on Port {Port}");
                Console.ReadKey();
            }
            catch (IOException ex)
            {
                Console.WriteLine($"There was an error running the server", ex.Message);
                throw;
            }
            finally
            {
                if (server != null)
                {
                    server.ShutdownAsync().Wait();
                }
            }
        }
    }
}

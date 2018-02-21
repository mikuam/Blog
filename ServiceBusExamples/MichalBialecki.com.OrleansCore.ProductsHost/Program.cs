using MichalBialecki.com.OrleansCore.ProductGrainInterfaces;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Orleans;
using Orleans.Hosting;
using Orleans.Runtime.Configuration;
using System;
using System.Text;
using System.Threading.Tasks;

namespace MichalBialecki.com.OrleansCore.ProductsHost
{
    class Program
    {
        public static int Main(string[] args)
        {
            return RunMainAsync().Result;
        }

        private static async Task<int> RunMainAsync()
        {
            try
            {
                var host = await StartSilo();
                Console.WriteLine("Press Enter to terminate...");
                Console.ReadLine();

                await host.StopAsync();
                RunListener();

                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return 1;
            }
        }

        private static async Task<ISiloHost> StartSilo()
        {
            // define the cluster configuration
            var config = ClusterConfiguration.LocalhostPrimarySilo();
            config.AddMemoryStorageProvider();

            var builder = new SiloHostBuilder()
                .UseConfiguration(config)
                //.ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(HelloGrain).Assembly).WithReferences())
                .ConfigureLogging(logging => logging.AddConsole());

            var host = builder.Build();
            await host.StartAsync();
            return host;
        }

        private static void RunListener()
        {   
            var client = new ServiceBusCore.ServiceBusClient();
            client.Init(" ", string.Empty, "productRatingUpdates", ReceiveMode.PeekLock);
            var subscriptionClient = client.GetSubscriptionClient("consumerOrleansCore");

            while (true)
            {
                try
                {
                    subscriptionClient.RegisterMessageHandler(
                        async (message, token) =>
                        {
                            var messageJson = Encoding.UTF8.GetString(message.Body);
                            var updateMessage = JsonConvert.DeserializeObject<ProductRatingUpdateMessage>(messageJson);

                            var productGrain = GrainClient.GrainFactory.GetGrain<IProductRatingGrain>(updateMessage.ProductId);
                            productGrain.

                            await subscriptionClient.CompleteAsync(message.SystemProperties.LockToken);
                        },
                        new MessageHandlerOptions(async args => Console.WriteLine(args.Exception))
                            { MaxConcurrentCalls = 1, AutoComplete = false });
                }
                catch (Exception)
                {
                    
                }
            }
        }
    }
}

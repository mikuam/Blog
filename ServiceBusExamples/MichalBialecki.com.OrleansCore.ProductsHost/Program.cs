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
        private const string ServiceBusConnectionString = "Endpoint=sb://bialecki.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=39cH/mE4siF49REMd9xtjVlUwoc0yPJNz9J8isRc9vY=";

        public static int Main(string[] args)
        {
            return RunMainAsync().Result;
        }

        private static async Task<int> RunMainAsync()
        {
            try
            {
                var host = await StartSilo();

                RunListener();
                Console.WriteLine("Press Enter to terminate...");
                Console.ReadLine();

                await host.StopAsync();

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
            client.Init(ServiceBusConnectionString, string.Empty, "productRatingUpdates", ReceiveMode.PeekLock);
            var subscriptionClient = client.GetSubscriptionClient("consumerOrleansCore");

            try
            {
                subscriptionClient.RegisterMessageHandler(
                    async (message, token) =>
                    {
                        var messageJson = Encoding.UTF8.GetString(message.Body);
                        var updateMessage = JsonConvert.DeserializeObject<ProductRatingUpdateMessage>(messageJson);

                        var productGrain = GrainClient.GrainFactory.GetGrain<IProductRatingGrain>(updateMessage.ProductId);
                        await productGrain.UpdateRatingAsync(
                            updateMessage.RatingSum,
                            updateMessage.RatingCount,
                            updateMessage.SellerId);

                        await subscriptionClient.CompleteAsync(message.SystemProperties.LockToken);
                    },
                    new MessageHandlerOptions(async args => Console.WriteLine(args.Exception))
                        { MaxConcurrentCalls = 1, AutoComplete = false });
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
        }
    }
}

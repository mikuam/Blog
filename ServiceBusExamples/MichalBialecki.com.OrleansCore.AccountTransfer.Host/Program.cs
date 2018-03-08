using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Hosting;
using Orleans.Hosting.Development;
using Orleans.Configuration;
using System.Net;

namespace MichalBialecki.com.OrleansCore.AccountTransfer.Host
{
    public class Program
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
            var siloPort = 11111;
            int gatewayPort = 30000;
            var siloAddress = IPAddress.Loopback;

            var builder = new SiloHostBuilder()
                .Configure(options => options.ClusterId = "accounting")
                .UseDevelopmentClustering(options => options.PrimarySiloEndpoint = new IPEndPoint(siloAddress, siloPort))
                .ConfigureEndpoints(siloAddress, siloPort, gatewayPort)
                .ConfigureApplicationParts(parts => parts.AddFromAppDomain().AddFromApplicationBaseDirectory())
                .ConfigureLogging(logging => logging.AddConsole())
                .AddMemoryGrainStorageAsDefault()
                .UseInClusterTransactionManager()
                .UseInMemoryTransactionLog()
                .UseTransactionalState();

            var host = builder.Build();
            await host.StartAsync();
            return host;
        }
    }

/*
        private const string ServiceBusConnectionString = "Endpoint=sb://bialecki.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=39cH/mE4siF49REMd9xtjVlUwoc0yPJNz9J8isRc9vY=";

            var client = new ServiceBusCore.ServiceBusClient();
            client.Init(ServiceBusConnectionString, string.Empty, "productRatingUpdates", ReceiveMode.PeekLock);
            var subscriptionClient = client.GetSubscriptionClient("sampleSubscription");

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
    */
}

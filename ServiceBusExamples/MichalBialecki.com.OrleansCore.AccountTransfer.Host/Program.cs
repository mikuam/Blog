using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Hosting;
using Orleans.Hosting.Development;
using Orleans.Configuration;
using System.Net;
using Microsoft.Extensions.DependencyInjection;
using MichalBialecki.com.OrleansCore.AccountTransfer.Grains;
using MichalBialecki.com.OrleansCore.AccountTransfer.Interfaces;

namespace MichalBialecki.com.OrleansCore.AccountTransfer.Host
{
    public class Program
    {
        private const string CosmosBDConnectionString = "DefaultEndpointsProtocol=https;AccountName=bialecki-t;AccountKey=lDRHuoOFmq3g91orbG1L68YOu7M5DXRgXG8DoHU771IVvNPaijdBEyBuuNh5YY9YkJQXhTU8AJIbEEseC8ZTHg==;TableEndpoint=https://bialecki-t.table.cosmosdb.azure.com:443/;";

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
            var builder = new SiloHostBuilder()
                .UseLocalhostClustering()
                .Configure<EndpointOptions>(options => options.AdvertisedIPAddress = IPAddress.Loopback)
                .ConfigureServices(context => ConfigureDI(context))
                .ConfigureLogging(logging => logging.AddConsole())
                .UseInClusterTransactionManager()
                .UseInMemoryTransactionLog()
                .AddAzureTableGrainStorageAsDefault(
                    (options) => {
                        options.ConnectionString = CosmosBDConnectionString;
                        options.UseJson = true;
                    })
                .UseTransactionalState();

            var host = builder.Build();
            await host.StartAsync();
            return host;
        }

        private static IServiceProvider ConfigureDI(IServiceCollection services)
        {
            services.AddSingleton<IServiceBusClient, ServiceBusClient>();

            return services.BuildServiceProvider();
        }
    }
}

using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Runtime;
using System.Net;
using MichalBialecki.com.OrleansCore.AccountTransfer.Interfaces;
using Microsoft.Azure.ServiceBus;
using System.Text;
using Newtonsoft.Json;

namespace MichalBialecki.com.OrleansCore.AccountTransfer.Client
{
    /// <summary>
    /// Orleans test silo client
    /// </summary>
    public class Program
    {
        private const string ServiceBusConnectionString = "Endpoint=sb://bialecki.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=39cH/mE4siF49REMd9xtjVlUwoc0yPJNz9J8isRc9vY=";

        static int Main(string[] args)
        {
            return RunMainAsync().Result;
        }

        private static async Task<int> RunMainAsync()
        {
            try
            {
                using (var client = await StartClientWithRetries())
                {
                    await DoClientWork(client);
                    Console.ReadKey();
                }

                return 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return 1;
            }
        }

        private static async Task<IClusterClient> StartClientWithRetries(int initializeAttemptsBeforeFailing = 5)
        {
            int attempt = 0;
            IClusterClient client;
            while (true)
            {
                try
                {
                    int gatewayPort = 30000;
                    var siloAddress = IPAddress.Loopback;
                    var gateway = new IPEndPoint(siloAddress, gatewayPort);

                    client = new ClientBuilder()
                        .ConfigureCluster(options => options.ClusterId = "accounting")
                        .UseStaticClustering(options => options.Gateways.Add(gateway.ToGatewayUri()))
                        .ConfigureApplicationParts(parts => parts.AddFromAppDomain().AddFromApplicationBaseDirectory())
                        .ConfigureLogging(logging => logging.AddConsole())
                        .Build();

                    await client.Connect();
                    Console.WriteLine("Client successfully connect to silo host");
                    break;
                }
                catch (SiloUnavailableException)
                {
                    attempt++;
                    Console.WriteLine($"Attempt {attempt} of {initializeAttemptsBeforeFailing} failed to initialize the Orleans client.");
                    if (attempt > initializeAttemptsBeforeFailing)
                    {
                        throw;
                    }
                    await Task.Delay(TimeSpan.FromSeconds(4));
                }
            }

            return client;
        }

        private static async Task DoClientWork(IClusterClient client)
        {
            var subscriptionClient = new SubscriptionClient(ServiceBusConnectionString, "accountTransferUpdates", "orleansSubscription");
            subscriptionClient.PrefetchCount = 1000;

            try
            {
                subscriptionClient.RegisterMessageHandler(
                    async (message, token) =>
                    {
                        try
                        {
                            var messageJson = Encoding.UTF8.GetString(message.Body);
                            var updateMessage = JsonConvert.DeserializeObject<AccountTransferMessage>(messageJson);
                            
                            await client.GetGrain<IAccountGrain>(updateMessage.From).Withdraw(updateMessage.Amount);
                            await client.GetGrain<IAccountGrain>(updateMessage.To).Deposit(updateMessage.Amount);

                            await subscriptionClient.CompleteAsync(message.SystemProperties.LockToken);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                    },
                    new MessageHandlerOptions(async args => Console.WriteLine(args.Exception + ", stack trace: " + args.Exception.StackTrace))
                    { MaxConcurrentCalls = 30, AutoComplete = false });
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
        }
    }
}

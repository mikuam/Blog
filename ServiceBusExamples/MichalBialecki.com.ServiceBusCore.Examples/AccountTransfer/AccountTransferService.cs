using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace MichalBialecki.com.ServiceBusCore.Examples.AccountTransfer
{
    public class AccountTransferService
    {
        private const string ServiceBusKey = "ServiceBusConnectionString";

        private readonly IConfigurationRoot _configuration;

        public AccountTransferService(IConfigurationRoot configuration)
        {
            _configuration = configuration;
        }

        public void Run()
        {
            var service = new TableStorageService(_configuration);

            try
            {
                var subscriptionClient = new SubscriptionClient(
                    _configuration[ServiceBusKey],
                    "accountTransferUpdates",
                    "commonSubscription");
                subscriptionClient.PrefetchCount = 1000;

                subscriptionClient.RegisterMessageHandler(
                    async (message, token) =>
                    {
                        var messageJson = Encoding.UTF8.GetString(message.Body);
                        var updateMessage = JsonConvert.DeserializeObject<AccountTransferMessage>(messageJson);

                        await service.UpdateAccount(updateMessage.From, -updateMessage.Amount);
                        await service.UpdateAccount(updateMessage.To, updateMessage.Amount);

                        Console.WriteLine($"Processed a message from {updateMessage.From} to {updateMessage.To}");
                    },
                    new MessageHandlerOptions(OnException)
                        { MaxConcurrentCalls = 1, AutoComplete = true });
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
        }

        private Task OnException(ExceptionReceivedEventArgs args)
        {
            Console.WriteLine(args.Exception);

            return Task.CompletedTask;
        }
    }
}

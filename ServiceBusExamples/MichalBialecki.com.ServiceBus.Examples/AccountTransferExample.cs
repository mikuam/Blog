using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ServiceBusExamples
{
    public class AccountTransferExample
    {
        private const string ServiceBusConnectionString = "Endpoint=sb://bialecki.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=39cH/mE4siF49REMd9xtjVlUwoc0yPJNz9J8isRc9vY=";

        private TopicClient accountBalanceClient;

        public void Process()
        {
            var accountTransferClient = SubscriptionClient.CreateFromConnectionString(
                ServiceBusConnectionString,
                "accountTransferUpdates",
                "commonSubscription");
            accountTransferClient.PrefetchCount = 1000;

            accountBalanceClient = TopicClient.CreateFromConnectionString(
                ServiceBusConnectionString, "balanceUpdates");

            accountTransferClient.OnMessageAsync(
                OnMessage,
                new OnMessageOptions { AutoComplete = false, MaxConcurrentCalls = 5 });
        }

        private async Task OnMessage(BrokeredMessage message)
        {
            try
            {
                // parse message
                var stream = message.GetBody<Stream>();
                var reader = new StreamReader(stream);
                string messageBody = reader.ReadToEnd();

                var transferMessage = JsonConvert.DeserializeObject<AccountTransferMessage>(messageBody);

                // get and update
                var fromBalance = TableStorageService.Instance.UpdateAccount(transferMessage.From, -(double)transferMessage.Amount);
                var toBalance = TableStorageService.Instance.UpdateAccount(transferMessage.To, (double)transferMessage.Amount);

                // send to topic
                var fromUpdateMessage = new BrokeredMessage(
                    JsonConvert.SerializeObject(
                        new BalanceUpdateMessage { AccountNumber = transferMessage.From, Balance = (decimal)fromBalance }));
                var toUpdateMessage = new BrokeredMessage(
                    JsonConvert.SerializeObject(
                        new BalanceUpdateMessage { AccountNumber = transferMessage.To, Balance = (decimal)toBalance }));

                await accountBalanceClient.SendBatchAsync(new[] { fromUpdateMessage, toUpdateMessage });

                await message.CompleteAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}

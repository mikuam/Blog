using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Text;

namespace MichalBialecki.com.ServiceBusCore.Examples
{
    public class MessageReceiver
    {
        private const string ServiceBusConnectionString = "Endpoint=sb://bialecki.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=39cH/mE4siF49REMd9xtjVlUwoc0yPJNz9J8isRc9vY=";

        public void Receive()
        {
            var subscriptionClient = new SubscriptionClient(ServiceBusConnectionString, "productRatingUpdates", "sampleSubscription");
            
            try
            {
                subscriptionClient.RegisterMessageHandler(
                    async (message, token) =>
                    {
                        var messageJson = Encoding.UTF8.GetString(message.Body);
                        var updateMessage = JsonConvert.DeserializeObject<ProductRatingUpdateMessage>(messageJson);

                        Console.WriteLine($"Received message with productId: {updateMessage.ProductId}");

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

        public void ReceiveAll()
        {
            var queueClient = new QueueClient(ServiceBusConnectionString, "go_testing");

            queueClient.RegisterMessageHandler(
                async (message, token) =>
                {
                    var messageBody = Encoding.UTF8.GetString(message.Body);

                    Console.WriteLine($"Received: {messageBody}, time: {DateTime.Now}");

                    await queueClient.CompleteAsync(message.SystemProperties.LockToken);
                },
                new MessageHandlerOptions(async args => Console.WriteLine(args.Exception))
                { MaxConcurrentCalls = 1, AutoComplete = false });
        }

        public void ReceiveOne()
        {
            var queueClient = new QueueClient(ServiceBusConnectionString, "go_testing");

            queueClient.RegisterMessageHandler(
                async (message, token) =>
                {
                    var messageBody = Encoding.UTF8.GetString(message.Body);

                    Console.WriteLine($"Received: {messageBody}, time: {DateTime.Now}");

                    await queueClient.CompleteAsync(message.SystemProperties.LockToken);

                    await queueClient.CloseAsync();
                },
                new MessageHandlerOptions(async args => Console.WriteLine(args.Exception))
                { MaxConcurrentCalls = 1, AutoComplete = false });
        }
    }
}

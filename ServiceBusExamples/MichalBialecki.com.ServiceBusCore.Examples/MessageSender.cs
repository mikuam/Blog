using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MichalBialecki.com.ServiceBusCore.Examples
{
    public class MessageSender
    {
        private const string ServiceBusConnectionString = "Endpoint=sb://bialecki.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=39cH/mE4siF49REMd9xtjVlUwoc0yPJNz9J8isRc9vY=";

        public async Task Send()
        {
            try
            {
                //var productRating = new ProductRatingUpdateMessage { ProductId = 123, RatingSum = 23 };
                //var message = new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(productRating))){ SessionId = "S1" };

                var topicClient = new TopicClient(ServiceBusConnectionString, "accountTransferUpdates");
                for(int i = 0; i < 10; i++)
                {
                    await topicClient.SendAsync(GetMessages());
                    Console.WriteLine($"Sending message batch {i}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private List<Message> GetMessages()
        {
            var rand = new Random();
            var messages = new List<Message>();
            for (int i = 0; i < 100; i++)
            {
                messages.Add(new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(
                    new { From = rand.Next(1, 1000), To = rand.Next(1, 1000), Amount = rand.Next(20, 800) }))));
            }

            return messages;
        }
    }
}

using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System;
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
                var productRating = new ProductRatingUpdateMessage { ProductId = 123, RatingSum = 23 };
                var message = new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(productRating))){ SessionId = "S1" };

                var transferMessage = new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(
                    new { From = 1, To = 2, Ammount = 300 })));
                var topicClient = new TopicClient(ServiceBusConnectionString, "accountTransferUpdates");
                await topicClient.SendAsync(message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}

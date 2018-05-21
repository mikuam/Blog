using System;
using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;

namespace MichalBialecki.com.ServiceBus.Examples
{
    public class MessageReceiverOld
    {
        private const string ServiceBusConnectionString = "Endpoint=sb://bialecki.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=39cH/mE4siF49REMd9xtjVlUwoc0yPJNz9J8isRc9vY=";

        public async Task ReceiveOne()
        {
            var queueClient = QueueClient.CreateFromConnectionString(ServiceBusConnectionString, "go_testing", ReceiveMode.PeekLock);

            var message = await queueClient.ReceiveAsync();
            Console.WriteLine($"Received: {message.GetBody<string>()}, time: {DateTime.Now}");

            await message.CompleteAsync();
        }
    }
}

using MichalBialecki.com.OrleansCore.AccountTransfer.Interfaces;
using Microsoft.Azure.ServiceBus;
using System.Threading.Tasks;

namespace MichalBialecki.com.OrleansCore.AccountTransfer.Grains
{
    public class ServiceBusClient : IServiceBusClient
    {
        private const string ServiceBusConnectionString = "Endpoint=sb://bialecki.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=39cH/mE4siF49REMd9xtjVlUwoc0yPJNz9J8isRc9vY=";

        private readonly TopicClient topicClient;

        public ServiceBusClient()
        {
            topicClient = new TopicClient(ServiceBusConnectionString, "balanceUpdates");
        }

        public async Task SendMessageAsync(Message message)
        {
            await topicClient.SendAsync(message);
        }
    }
}

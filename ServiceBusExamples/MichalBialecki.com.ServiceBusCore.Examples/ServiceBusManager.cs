using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Management;
using System.Threading.Tasks;

namespace MichalBialecki.com.ServiceBusCore.Examples
{
    public class ServiceBusManager
    {
        public async Task<IQueueClient> GetOrCreateQueue(string serviceBusConnectionString, string queueName)
        {
            var managementClient = new ManagementClient(serviceBusConnectionString);
            if (!(await managementClient.QueueExistsAsync(queueName)))
            {
                await managementClient.CreateQueueAsync(new QueueDescription(queueName));
            }

            var queueClient = new QueueClient(serviceBusConnectionString, queueName);
            return queueClient;
        }

        public async Task<ITopicClient> GetOrCreateTopic(string serviceBusConnectionString, string topicPath)
        {
            var managementClient = new ManagementClient(serviceBusConnectionString);
            if (!(await managementClient.TopicExistsAsync(topicPath)))
            {
                await managementClient.CreateTopicAsync(new TopicDescription(topicPath));
            }

            var topicClient = new TopicClient(serviceBusConnectionString, topicPath);
            return topicClient;
        }

        public async Task<ISubscriptionClient> GetOrCreateTopicSubscription(string serviceBusConnectionString, string topicPath, string subscriptionName)
        {
            var managementClient = new ManagementClient(serviceBusConnectionString);
#if DEBUG
            if (!(await managementClient.SubscriptionExistsAsync(topicPath, subscriptionName + "_MikTesting")))
            {
                await managementClient.CreateSubscriptionAsync(
                    new SubscriptionDescription(topicPath, subscriptionName + "_MikTesting")
                    {
                        EnableBatchedOperations = true,
                        AutoDeleteOnIdle = System.TimeSpan.FromDays(100),
                        EnableDeadLetteringOnMessageExpiration = true,
                        DefaultMessageTimeToLive = System.TimeSpan.FromDays(2),
                        MaxDeliveryCount = 5,
                        LockDuration = System.TimeSpan.FromMinutes(5)
                    });
            }
#else       
            if (!(await managementClient.SubscriptionExistsAsync(topicPath, subscriptionName)))
            {
                await managementClient.CreateSubscriptionAsync(
                new SubscriptionDescription(topicPath, subscriptionName)
                {
                    EnableBatchedOperations = true,
                    AutoDeleteOnIdle = System.TimeSpan.FromDays(100),
                    EnableDeadLetteringOnMessageExpiration = true,
                    DefaultMessageTimeToLive = System.TimeSpan.FromDays(100),
                    MaxDeliveryCount = 100,
                    LockDuration = System.TimeSpan.FromMinutes(5)
                });
            }
#endif
            var subscriptionClient = new SubscriptionClient(serviceBusConnectionString, topicPath, subscriptionName);
            return subscriptionClient;
        }
    }
}

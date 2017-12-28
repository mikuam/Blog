using System;
using Microsoft.Azure.ServiceBus;

namespace ServiceBusExamples.MessagesSender.Web
{
    public class ServiceBusHelper
    {
        public static QueueClient GetQueueClient(ReceiveMode receiveMode = ReceiveMode.ReceiveAndDelete)
        {
            const string queueName = "stockchangerequest";
            var queueClient = new QueueClient(ConfigurationHelper.GetServiceBusConnectionString(), queueName, receiveMode, GetRetryPolicy());
            return queueClient;
        }

        public static TopicClient GetTopicClient(string topicName = "stockupdated")
        {
            var topicClient = new TopicClient(ConfigurationHelper.GetServiceBusConnectionString(), topicName, GetRetryPolicy());
            return topicClient;
        }

        private static RetryExponential GetRetryPolicy()
        {
            return new RetryExponential(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(30), 10);
        }
    }
}

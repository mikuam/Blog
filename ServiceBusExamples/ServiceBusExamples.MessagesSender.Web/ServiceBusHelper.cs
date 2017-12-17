using System;
using Microsoft.Azure.ServiceBus;

namespace ServiceBusExamples.MessagesSender.Web
{
    public class ServiceBusHelper
    {
        private const string ServiceBusString =
                "Endpoint=sb://bialecki.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=39cH/mE4siF49REMd9xtjVlUwoc0yPJNz9J8isRc9vY=";

        public static QueueClient GetQueueClient(ReceiveMode receiveMode = ReceiveMode.ReceiveAndDelete)
        {
            const string queueName = "stockchangerequest";
            var queueClient = new QueueClient(ServiceBusString, queueName, receiveMode, GetRetryPolicy());
            return queueClient;
        }

        public static TopicClient GetTopicClient(string topicName = "stockupdated")
        {
            var topicClient = new TopicClient(ServiceBusString, topicName, GetRetryPolicy());
            return topicClient;
        }

        private static RetryExponential GetRetryPolicy()
        {
            return new RetryExponential(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(30), 10);
        }
    }
}

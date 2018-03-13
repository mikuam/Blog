using Microsoft.Azure.ServiceBus;
using System;

namespace MichalBialecki.com.ServiceBusCore
{
    public class ServiceBusClient
    {
        private string _serviceBusConnectionString;

        private string _queueName;

        private string _topicName;

        private ReceiveMode _receiveMode;

        public void Init(
            string serviceBusConnectionString,
            string queueName,
            string topicName,
            ReceiveMode receiveMode = ReceiveMode.ReceiveAndDelete)
        {
            _serviceBusConnectionString = serviceBusConnectionString;
            _queueName = queueName;
            _topicName = topicName;
            _receiveMode = receiveMode;
        }

        public IQueueClient GetQueueClient()
        {
            var queueClient = new QueueClient(_serviceBusConnectionString, _queueName, _receiveMode, GetRetryPolicy());
            return queueClient;
        }

        public ITopicClient GetTopicClient()
        {
            var topicClient = new TopicClient(_serviceBusConnectionString, _topicName, GetRetryPolicy());
            return topicClient;
        }

        public SubscriptionClient GetSubscriptionClient(string subscriptionName)
        {
            var subscriptionClient = new SubscriptionClient(
                _serviceBusConnectionString,
                _topicName,
                subscriptionName,
                _receiveMode,
                GetRetryPolicy());

            return subscriptionClient;
        }

        private static RetryExponential GetRetryPolicy()
        {
            return new RetryExponential(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(30), 10);
        }
    }
}

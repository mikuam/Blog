using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;

namespace ServiceBusExamples.MessagesSender.NetCore.Web.Services
{
    public class SimpleBufferMessagesService
    {
        private const string ServiceBusConnectionString = "Endpoint=sb://bialecki.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=39cH/mE4siF49REMd9xtjVlUwoc0yPJNz9J8isRc9vY=";

        private static readonly List<Message> _messages = new List<Message>();

        private static DateTime _lastMessageSent = DateTime.Now;

        private readonly TopicClient _topicClient;

        public SimpleBufferMessagesService()
        {
            _topicClient = new TopicClient(ServiceBusConnectionString, "accountTransferUpdates");
        }

        public async Task AddMessage(string message)
        {
            _messages.Add(new Message(Encoding.UTF8.GetBytes(message)));

            if (_messages.Count >= 10
                || DateTime.Now - _lastMessageSent > TimeSpan.FromSeconds(20))
            {
                await SendMessages(_messages);
                _messages.Clear();
                _lastMessageSent = DateTime.Now;
            }
        }

        private async Task SendMessages(List<Message> messages)
        {
            await _topicClient.SendAsync(messages);
        }
    }
}

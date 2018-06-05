using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;

namespace ServiceBusExamples.MessagesSender.NetCore.Web.Services
{
    public class TimerBufferMessagesService
    {
        private const string ServiceBusConnectionString = "Endpoint=sb://bialecki.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=39cH/mE4siF49REMd9xtjVlUwoc0yPJNz9J8isRc9vY=";

        private static readonly ICollection<Message> _messages = new List<Message>();

        private readonly TopicClient _topicClient;

        public TimerBufferMessagesService()
        {
            _topicClient = new TopicClient(ServiceBusConnectionString, "accountTransferUpdates");
        }

        public void AddMessage(string message)
        {
            lock (((ICollection) _messages).SyncRoot)
            {
                _messages.Add(new Message(Encoding.UTF8.GetBytes(message)));
            }
        }

        public void SendMessages()
        {
            if (_messages.Count == 0)
            {
                return;
            }

            List<Message> localMessages;
            lock (((ICollection)_messages).SyncRoot)
            {
                localMessages = new List<Message>(_messages);
                _messages.Clear();
            }

            Task.Run(async () => { await _topicClient.SendAsync(localMessages); });
        }
    }
}

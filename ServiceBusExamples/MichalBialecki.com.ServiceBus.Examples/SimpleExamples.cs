using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBusExamples
{
    public class SimpleExamples
    {
        private static void SimpleSendBatch()
        {
            var client = GetQueueClient();
            client.SendBatch(GetALotOfMessages());
        }

        public static void SimpleAndSmartSendBatch()
        {
            var client = GetQueueClient();
            var messages = GetALotOfMessages();

            const int maxBatchSizeInBytes = 256000;
            var i = 0;
            long currentBatchSize = 0;
            var listToSend = new List<BrokeredMessage>();
            while (i < messages.Count)
            {
                Console.WriteLine($"Standard size: {messages[i].Size}, custom size: {GetMessageSize(messages[i])}");

                if (currentBatchSize + messages[i].Size < maxBatchSizeInBytes)
                {
                    listToSend.Add(messages[i]);
                    currentBatchSize += messages[i].Size;
                }
                else
                {
                    client.SendBatch(listToSend);
                    listToSend.Clear();
                    listToSend.Add(messages[i]);
                    currentBatchSize = messages[i].Size;
                }

                i++;
            }

            if (listToSend.Any())
            {
                client.SendBatch(listToSend);
            }
        }

        private static long GetMessageSize(BrokeredMessage message)
        {
            var customPropertiesSize = message.Properties.Sum(p => p.Key.Length + GetObjectSize(p.Value));

            var standardPropertiesSize = message.ContentType?.Length ?? 0;
            standardPropertiesSize += message.CorrelationId?.Length ?? 0;
            standardPropertiesSize += message.DeadLetterSource?.Length ?? 0;
            standardPropertiesSize += message.Label?.Length ?? 0;
            standardPropertiesSize += message.MessageId?.Length ?? 0;
            standardPropertiesSize += message.PartitionKey?.Length ?? 0;
            standardPropertiesSize += message.ReplyTo?.Length ?? 0;
            standardPropertiesSize += message.ReplyToSessionId?.Length ?? 0;
            standardPropertiesSize += message.SessionId?.Length ?? 0;
            standardPropertiesSize += message.To?.Length ?? 0;
            standardPropertiesSize += message.ViaPartitionKey?.Length ?? 0;

            return message.Size + customPropertiesSize + standardPropertiesSize;
        }

        private static long GetObjectSize(object o)
        {
            using (Stream s = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(s, o);
                return s.Length;
            }
        }

        private static void SendMessagesInBatch()
        {
            try
            {
                var messages = GetALotOfMessages();
                var messages2 = GetALotOfMessages();

                var client = GetQueueClient();
                var s = new Stopwatch();
                s.Start();
                client.SendBatch(messages);
                s.Stop();
                Console.WriteLine($"Batch send: {s.ElapsedMilliseconds} miliseconds");

                s.Reset();
                s.Start();
                foreach (var item in messages2)
                {
                    client.Send(item);
                }

                s.Stop();
                Console.WriteLine($"Sequential send: {s.ElapsedMilliseconds} miliseconds");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static List<BrokeredMessage> GetALotOfMessages()
        {
            var messages = new List<BrokeredMessage>();
            for (int i = 0; i < 1000; i++)
            {
                messages.Add(
                    new BrokeredMessage(new string[100])
                    {
                        ContentType = "This is my content type, very unique",
                        ReplyTo = "Myself and I",
                        To = "Me dear friend that I really need to send it to, Me dear friend that I really need to send it to, Me dear friend that I really need to send it to, Me dear friend that I really need to send it to, Me dear friend that I really need to send it to"
                    });
            }

            return messages;
        }

        static void GetMessagesBySubscribing()
        {
            var queueClient = GetQueueClient();
            var options = new OnMessageOptions
            {
                AutoComplete = true,
                MaxConcurrentCalls = 5
            };
            options.ExceptionReceived += OptionsOnExceptionReceived;
            queueClient.OnMessage(OnMessage, options);
        }

        private static async Task StartListenLoopWithDeferral()
        {
            var client = GetQueueClient(ReceiveMode.PeekLock);
            var deferredMessages = new List<KeyValuePair<long, DateTimeOffset>>();

            while (true)
            {
                var messages = Enumerable.Empty<BrokeredMessage>();

                try
                {
                    messages = await client.ReceiveBatchAsync(10, TimeSpan.FromSeconds(10));
                    messages = messages ?? Enumerable.Empty<BrokeredMessage>();
                    if (!messages.Any())
                    {
                        continue;
                    }


                    var rand = new Random();
                    if (rand.Next(1, 5) == 1)
                    {
                        throw new ArgumentOutOfRangeException("Sorry, an error occured");
                    }

                    // processing
                    foreach (var message in messages)
                    {
                        Console.WriteLine("Received a message: " + message.GetBody<string>());
                    }
                    await client.CompleteBatchAsync(messages.Select(m => m.LockToken));

                    // handling dererred messages
                    var messagesToProcessAgain = deferredMessages.Where(d => d.Value < DateTime.Now).Take(10).ToList();
                    foreach (var messageToProcess in messagesToProcessAgain)
                    {
                        BrokeredMessage message = null;
                        try
                        {
                            deferredMessages.Remove(messageToProcess);
                            message = await client.ReceiveAsync(messageToProcess.Key);

                            if (message != null)
                            {
                                // processing
                                Console.WriteLine("Received a message: " + message.GetBody<string>());

                                await client.CompleteAsync(message.LockToken);
                            }
                        }
                        catch (MessageNotFoundException) { }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            deferredMessages.Add(new KeyValuePair<long, DateTimeOffset>(
                                message.SequenceNumber,
                                DateTimeOffset.Now + TimeSpan.FromMinutes(2)));
                        }
                    }
                }
                catch (MessageLockLostException e)
                {
                    Console.WriteLine(e);

                    foreach (var message in messages)
                    {
                        await message.AbandonAsync();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);

                    // defer messages
                    foreach (var message in messages)
                    {
                        deferredMessages.Add(new KeyValuePair<long, DateTimeOffset>(
                            message.SequenceNumber,
                            DateTimeOffset.Now + TimeSpan.FromMinutes(2)));
                        await message.DeferAsync();
                    }
                }
            }
        }

        private static void OptionsOnExceptionReceived(object sender, ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            if (exceptionReceivedEventArgs?.Exception != null)
            {
                if (!(exceptionReceivedEventArgs.Exception is MessagingException && ((MessagingException)exceptionReceivedEventArgs.Exception).IsTransient))
                {
                    Console.WriteLine("Exception occured: " + exceptionReceivedEventArgs.Exception.Message);
                }
            }
        }

        private static void OnMessage(BrokeredMessage brokeredMessage)
        {
            throw new MessagingCommunicationException("new exception");

            try
            {
                Console.WriteLine("Received a message: " + brokeredMessage.GetBody<string>());
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception occured while processing: " + e.Message);
            }
        }

        static async Task GetMessagesWithPeekLock()
        {
            var queueClient = GetQueueClient(ReceiveMode.PeekLock);

            while (true)
            {
                try
                {
                    var messages = await queueClient.ReceiveBatchAsync(50);

                    try
                    {
                        // processing
                        foreach (var message in messages)
                        {
                            Console.WriteLine("Received a message: " + message.GetBody<string>());
                        }

                        await queueClient.CompleteBatchAsync(messages.Select(m => m.LockToken));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("An exception occured while processing: " + e.Message);
                        foreach (var message in messages)
                        {
                            await queueClient.AbandonAsync(message.LockToken);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An exception occured: " + ex.Message);
                }
            }
        }

        static async Task GetMessage()
        {
            var queueClient = GetQueueClient();

            while (true)
            {
                try
                {
                    var message = await queueClient.ReceiveAsync();

                    Console.WriteLine("Received a message: " + message.GetBody<string>());
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An exception occured: " + ex.Message);
                }
            }
        }

        private static QueueClient GetQueueClient(ReceiveMode receiveMode = ReceiveMode.ReceiveAndDelete)
        {
            const string queueName = "stockchangerequest";
            var connectionString = ConfigurationManager.AppSettings["Microsoft.ServiceBus.ConnectionString"];
            var queueClient = QueueClient.CreateFromConnectionString(connectionString, queueName, receiveMode);
            queueClient.RetryPolicy = new RetryExponential(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(30), 10);
            return queueClient;
        }

        static async Task SendMessageAsync()
        {
            try
            {
                var queueName = "stockchangerequest";
                var connectionString = ConfigurationManager.AppSettings["Microsoft.ServiceBus.ConnectionString"];
                var queueClient = QueueClient.CreateFromConnectionString(connectionString, queueName);
                queueClient.RetryPolicy = new RetryExponential(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(30), 10);

                await queueClient.SendAsync(new BrokeredMessage("This is a test message content"));
            }
            catch (Exception ex)
            {
                Console.WriteLine("An exception occured: " + ex.Message);
            }
        }

        static async Task SendTestMessagesAsync(int numberOfMessages)
        {
            try
            {
                var queueName = "stockchangerequest";
                var connectionString = ConfigurationManager.AppSettings["Microsoft.ServiceBus.ConnectionString"];
                var queueClient = QueueClient.CreateFromConnectionString(connectionString, queueName);
                queueClient.RetryPolicy = new RetryExponential(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(30), 10);

                var messages = new List<BrokeredMessage>();
                for (var num = 0; num < numberOfMessages; num++)
                {
                    messages.Add(new BrokeredMessage($"This is a {num} test message content"));
                }

                await queueClient.SendBatchAsync(messages);

            }
            catch (Exception ex)
            {
                Console.WriteLine("An exception occured: " + ex.Message);
            }
        }
    }
}

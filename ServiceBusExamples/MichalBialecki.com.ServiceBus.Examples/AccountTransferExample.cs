using Microsoft.Azure.CosmosDB.Table;
using Microsoft.Azure.Storage;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceBusExamples
{
    public class AccountTransferExample
    {
        private const string ServiceBusConnectionString = "Endpoint=sb://bialecki.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=39cH/mE4siF49REMd9xtjVlUwoc0yPJNz9J8isRc9vY=";
        private const string CosmosBDConnectionString = "DefaultEndpointsProtocol=https;AccountName=bialecki-t;AccountKey=lDRHuoOFmq3g91orbG1L68YOu7M5DXRgXG8DoHU771IVvNPaijdBEyBuuNh5YY9YkJQXhTU8AJIbEEseC8ZTHg==;TableEndpoint=https://bialecki-t.table.cosmosdb.azure.com:443/;";
        //private const string CosmosDBPrimaryKey = "6hP3HMQeuH2n1oN0cby3W7rt9uZexEGgj7TNHwLnFgQh7qKqTvmEUQPsgahpBHgXPS9W0jTLRjS4G3l2HEPsPQ==";
        private const string CosmosDbEndpointUri = "https://bialecki-t.documents.azure.com:443/";
        private const string TableName = "Accounts";
        private const string PartitionKey = "Partition1";
        private const string AccountName = "bialecki-t";
        private const string AccountKey = "lDRHuoOFmq3g91orbG1L68YOu7M5DXRgXG8DoHU771IVvNPaijdBEyBuuNh5YY9YkJQXhTU8AJIbEEseC8ZTHg==";
        //DefaultEndpointsProtocol=https;AccountName=bialecki-t;AccountKey=lDRHuoOFmq3g91orbG1L68YOu7M5DXRgXG8DoHU771IVvNPaijdBEyBuuNh5YY9YkJQXhTU8AJIbEEseC8ZTHg==;TableEndpoint=https://bialecki-t.table.cosmosdb.azure.com:443/;

        private TopicClient accountBalanceClient;
        private CloudTable accountsTable;
        private IEnumerable<CloudTable> tables;

        public void Process()
        {
            var accountTransferClient = SubscriptionClient.CreateFromConnectionString(
                ServiceBusConnectionString,
                "accountTransferUpdates",
                "commonSubscription");
            accountTransferClient.PrefetchCount = 1000;

            accountBalanceClient = TopicClient.CreateFromConnectionString(
                ServiceBusConnectionString, "balanceUpdates");
            
            var storageAccount = CloudStorageAccount.Parse(CosmosBDConnectionString);
            var tableClient = storageAccount.CreateCloudTableClient();
            tables = tableClient.ListTables().ToList();
            accountsTable = tableClient.GetTableReference("accounts");
            
            accountTransferClient.OnMessageAsync(
                OnMessage,
                new OnMessageOptions { AutoComplete = false, MaxConcurrentCalls = 1 });
        }

        private async Task OnMessage(BrokeredMessage message)
        {
            try
            {
                // parse message
                var stream = message.GetBody<Stream>();
                var reader = new StreamReader(stream);
                string messageBody = reader.ReadToEnd();

                var transferMessage = JsonConvert.DeserializeObject<AccountTransferMessage>(messageBody);

                // get and update
                var fromBalance = await UpdateAccount(transferMessage.From, -(double)transferMessage.Amount);
                var toBalance = await UpdateAccount(transferMessage.To, (double)transferMessage.Amount);

                // send to topic
                var fromUpdateMessage = new BrokeredMessage(
                    JsonConvert.SerializeObject(
                        new BalanceUpdateMessage { AccountNumber = transferMessage.From, Balance = (decimal)fromBalance }));
                var toUpdateMessage = new BrokeredMessage(
                    JsonConvert.SerializeObject(
                        new BalanceUpdateMessage { AccountNumber = transferMessage.To, Balance = (decimal)toBalance }));

                await accountBalanceClient.SendBatchAsync(new[] { fromUpdateMessage, toUpdateMessage });

                await message.CompleteAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private async Task<double> UpdateAccount(int accountNumber, double amount)
        {
            var getOperation = TableOperation.Retrieve<AccountEntity>(PartitionKey, accountNumber.ToString());
            var result = await accountsTable.ExecuteAsync(getOperation);
            if (result.Result != null)
            {
                var account = result.Result as AccountEntity;
                account.Balance += amount;
                var replaceOperation = TableOperation.Replace(account);
                await accountsTable.ExecuteAsync(replaceOperation);

                return account.Balance;
            }
            else
            {
                var account = new AccountEntity
                {
                    PartitionKey = PartitionKey,
                    RowKey = accountNumber.ToString(),
                    Balance = amount
                };
                await accountsTable.ExecuteAsync(TableOperation.Insert(account));

                return amount;
            }
        }
    }
}

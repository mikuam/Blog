using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace MichalBialecki.com.ServiceBusCore.Examples.AccountTransfer
{
    public class TableStorageService
    {
        private const string EndpointUriKey = "CosmosDbEndpointUri";
        private const string PrimaryKeyKey = "CosmosDbPrimaryKey";
        private const string ServiceBusKey = "ServiceBusConnectionString";

        private readonly DocumentClient client;
        private readonly TopicClient topic;

        public TableStorageService(IConfigurationRoot configuration)
        {
            client = new DocumentClient(new Uri(configuration[EndpointUriKey]), configuration[PrimaryKeyKey]);
            topic = new TopicClient(configuration[ServiceBusKey], "balanceUpdates");
        }
        
        public async Task UpdateAccount(int accountNumber, decimal amount)
        {
            Account document;
            try
            {
                var response = await client.ReadDocumentAsync<Account>(accountNumber.ToString());
                document = response.Document;
                document.Balance += amount;
                await client.ReplaceDocumentAsync(accountNumber.ToString(), document);
            }
            catch (DocumentClientException de)
            {
                if (de.StatusCode == HttpStatusCode.NotFound)
                {
                    document = new Account { Id = accountNumber.ToString(), Balance = amount };
                    await client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri("bialecki", "accounts"), document);
                }
                else
                {
                    throw;
                }
            }

            await NotifyBalanceUpdate(accountNumber, document.Balance);
        }

        private async Task NotifyBalanceUpdate(int accountNumber, decimal balance)
        {
            var balanceUpdate = new BalanceUpdateMessage
            {
                AccountNumber = accountNumber,
                Balance = balance
            };

            var message = new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(balanceUpdate)));
            await topic.SendAsync(message);
        }
    }
}

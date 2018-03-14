using Microsoft.Azure.CosmosDB.Table;
using Microsoft.Azure.Storage;
using System;
using System.Threading.Tasks;

namespace ServiceBusExamples
{
    public class TableStorageService
    {
        private static readonly Lazy<TableStorageService> lazy =
            new Lazy<TableStorageService>(() => new TableStorageService());
        private static readonly object _lock = new object();

        private const string CosmosBDConnectionString = "DefaultEndpointsProtocol=https;AccountName=bialecki-t;AccountKey=lDRHuoOFmq3g91orbG1L68YOu7M5DXRgXG8DoHU771IVvNPaijdBEyBuuNh5YY9YkJQXhTU8AJIbEEseC8ZTHg==;TableEndpoint=https://bialecki-t.table.cosmosdb.azure.com:443/;";
        private const string PartitionKey = "Partition1";
        
        private CloudTable accountsTable;
        
        public static TableStorageService Instance { get { return lazy.Value; } }

        public TableStorageService()
        {
            var storageAccount = CloudStorageAccount.Parse(CosmosBDConnectionString);
            var tableClient = storageAccount.CreateCloudTableClient();
            accountsTable = tableClient.GetTableReference("accounts");

        }
        
        public double UpdateAccount(int accountNumber, double amount)
        {
            lock(_lock)
            {
                return UpdateAccountThreadSafe(accountNumber, amount);
            }
        }

        private double UpdateAccountThreadSafe(int accountNumber, double amount)
        {
            var getOperation = TableOperation.Retrieve<AccountEntity>(PartitionKey, accountNumber.ToString());
            var result = accountsTable.Execute(getOperation);
            if (result.Result != null)
            {
                var account = result.Result as AccountEntity;
                account.Balance += amount;
                var replaceOperation = TableOperation.Replace(account);
                accountsTable.Execute(replaceOperation);

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
                accountsTable.Execute(TableOperation.Insert(account));

                return amount;
            }
        }
    }
}

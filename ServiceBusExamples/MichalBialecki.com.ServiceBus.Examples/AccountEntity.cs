using Microsoft.Azure.CosmosDB.Table;

namespace ServiceBusExamples
{
    public class AccountEntity : TableEntity
    {
        public AccountEntity() { }

        public AccountEntity(string partition, string accountNumber)
        {
            PartitionKey = partition;
            RowKey = accountNumber;
        }

        public double Balance { get; set; }
    }
}

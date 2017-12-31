using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Documents.Client;
using ServiceBusExamples.MessagesSender.Web.Dto;

namespace ServiceBusExamples.MessagesSender.Web
{
    public class DocumentDbService
    {
        private const string DatabaseName = "Documents";

        private const string CollectionName = "Messages";

        public async Task SaveDocumentAsync(DocumentDto document)
        {
            try
            {
                var client = new DocumentClient(new Uri(ConfigurationHelper.GetCosmosDbEndpointUri()), ConfigurationHelper.GetCosmosDbPrimaryKey());
                await client.UpsertDocumentAsync(UriFactory.CreateDocumentCollectionUri(DatabaseName, CollectionName), document);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}, Message: {1}", e.Message, e.GetBaseException().Message);
            }
        }

        public IQueryable<DocumentDto> GetLatestDocuments()
        {
            try
            {
                var client = new DocumentClient(new Uri(ConfigurationHelper.GetCosmosDbEndpointUri()), ConfigurationHelper.GetCosmosDbPrimaryKey());
                return client.CreateDocumentQuery<DocumentDto>(
                    UriFactory.CreateDocumentCollectionUri(DatabaseName, CollectionName),
                    "SELECT * FROM Messages ORDER BY Messages.UpdatedAt desc",
                    new FeedOptions { MaxItemCount = 10 });
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}, Message: {1}", e.Message, e.GetBaseException().Message);
                return null;
            }
        }
    }
}

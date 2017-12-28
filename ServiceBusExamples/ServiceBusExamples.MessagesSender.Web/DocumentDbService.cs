using System;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using ServiceBusExamples.MessagesSender.Web.Dto;

namespace ServiceBusExamples.MessagesSender.Web
{
    public static class DocumentDbService
    {
        private const string DatabaseName = "Documents";

        private const string CollectionName = "Messages";

        public static async Task SaveDocumentAsync(DocumentDto document)
        {
            try
            {
                var client = new DocumentClient(new Uri(ConfigurationHelper.GetCosmosDbEndpointUri()), ConfigurationHelper.GetCosmosDbPrimaryKey());
                await client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(DatabaseName, CollectionName), document);
            }
            catch (DocumentClientException de)
            {
                Console.WriteLine("{0} error occurred: {1}, Message: {2}", de.StatusCode, de.Message, de.GetBaseException().Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}, Message: {1}", e.Message, e.GetBaseException().Message);
            }
        }
    }
}

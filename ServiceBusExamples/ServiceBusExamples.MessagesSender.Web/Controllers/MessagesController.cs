using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MichalBialecki.com.ServiceBusCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using ServiceBusExamples.MessagesSender.NetCore.Web.Dto;

namespace ServiceBusExamples.MessagesSender.NetCore.Web.Controllers
{
    [Route("Messages")]
    public class MessagesController : Controller
    {
        [Route("Send")]
        [HttpPost]
        public async Task<IActionResult> Send([FromBody]SendMessageDto message)
        {
            try
            {
                var document = new DocumentDto
                {
                    StockId = message.StockId,
                    Name = message.Name,
                    Price = message.Price,
                    UpdatedAt = DateTime.UtcNow
                };

                var serviceBusClient = new ServiceBusClient();
                serviceBusClient.Init(
                    ConfigurationHelper.GetServiceBusConnectionString(),
                    string.Empty,
                    "stockupdated");

                var topicClent = serviceBusClient.GetTopicClient();
                await topicClent.SendAsync(new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(document))));

                return StatusCode(200);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }

        [Route("Save")]
        [HttpPost]
        public async Task<IActionResult> Save([FromBody]SendMessageDto message)
        {
            try
            {
                var document = new DocumentDto
                {
                    StockId = message.StockId,
                    Name = message.Name,
                    Price = message.Price,
                    UpdatedAt = DateTime.UtcNow
                };

                await new DocumentDbService().SaveDocumentAsync(document);

                return StatusCode(200);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }

        [Route("GetTenLatestUpdates")]
        [HttpGet]
        public IQueryable<DocumentDto> GetTenLatestUpdates()
        {
            try
            {
                var documents = new DocumentDbService().GetLatestDocuments();

                return documents;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        [Route("SendProductRatingMessages")]
        [HttpPost]
        public async Task<IActionResult> SendProductRatingMessages([FromQuery]int numberOfMessages)
        {
            try
            {
                var ratingUpdates = ProductRatingUpdatesGenerator.GetMessages(numberOfMessages);
                var messages = ratingUpdates
                    .Select(m => new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(m))))
                    .ToList();

                var serviceBusClient = new ServiceBusClient();
                serviceBusClient.Init(
                    ConfigurationHelper.GetServiceBusConnectionString(),
                    string.Empty,
                    "productRatingUpdates");
                var topicClent = serviceBusClient.GetTopicClient();
                await topicClent.SendAsync(messages);

                return StatusCode(200);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }
    }
}

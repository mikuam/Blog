using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using ServiceBusExamples.MessagesSender.NetCore.Web.Dto;
using ServiceBusExamples.MessagesSender.NetCore.Web.Services;

namespace ServiceBusExamples.MessagesSender.NetCore.Web.Controllers
{
    [Route("Messages")]
    public class MessagesController : Controller
    {
        private const string ServiceBusConnectionString = "Endpoint=sb://bialecki.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=39cH/mE4siF49REMd9xtjVlUwoc0yPJNz9J8isRc9vY=";

        private readonly TopicClient _topicClient;

        public MessagesController()
        {
            _topicClient = new TopicClient(ServiceBusConnectionString, "stockupdated");
        }

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

                await _topicClient.SendAsync(new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(document))));

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

                await _topicClient.SendAsync(messages);

                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }

        [Route("SendWithBuffering")]
        [HttpPost]
        public IActionResult SendWithBuffering()
        {
            try
            {
                /*
                var service = new SimpleBufferMessagesService();
                await service.AddMessage("test message" + DateTime.Now);
                */            
    
                var service = new TimerBufferMessagesService();
                service.AddMessage("test message" + DateTime.Now);

                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }
    }
}

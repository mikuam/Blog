using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using ServiceBusExamples.MessagesSender.NetCore.Web.Dto;

namespace ServiceBusExamples.MessagesSender.NetCore.Web.Controllers
{
    public class MessagesController : Controller
    {
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

                var topicClent = ServiceBusHelper.GetTopicClient();
                await topicClent.SendAsync(new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(document))));

                return StatusCode(200);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }

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
    }
}

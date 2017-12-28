using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using ServiceBusExamples.MessagesSender.Web.Dto;

namespace ServiceBusExamples.MessagesSender.Web.Controllers
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

                await DocumentDbService.SaveDocumentAsync(document);

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
    }
}

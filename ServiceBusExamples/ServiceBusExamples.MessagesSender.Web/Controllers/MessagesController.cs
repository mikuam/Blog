using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.ServiceBus;

namespace ServiceBusExamples.MessagesSender.Web.Controllers
{
    public class MessagesController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> Send([FromBody]SendMessageDto mesaage)
        {
            try
            {
                var topicClent = ServiceBusHelper.GetTopicClient();
                await topicClent.SendAsync(new Message(Encoding.UTF8.GetBytes(mesaage.Value)));

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

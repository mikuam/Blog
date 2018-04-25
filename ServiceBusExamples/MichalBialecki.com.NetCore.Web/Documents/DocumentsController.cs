using Microsoft.AspNetCore.Mvc;

namespace MichalBialecki.com.NetCore.Web.Documents
{
    /*
    Sample XML file:
        <document>
	    <id>123456</id>
	    <content>This is document that I posted...</content>
	    <author>Michał Białecki</author>
	    <links>
		    <link>2345</link>
		    <link>5678</link>
	    </links>
    </document>

    and add header: Content-Type: text/xml
    
    Sample Json file:
    {
	    id: "1234",
	    content: "This is document that I posted...",
	    author: "Michał Białecki",
	    links: {
		    link: ["1234", "5678"]
	    }
    }

    and add header: Content-Type: application/json
    */

    [Route("api/Documents")]
    public class DocumentsController : Controller
    {
        [Route("SendDocument")]
        [HttpPost]
        public ActionResult SendDocument([FromBody]DocumentDto document)
        {
            return Ok();
        }
    }
}
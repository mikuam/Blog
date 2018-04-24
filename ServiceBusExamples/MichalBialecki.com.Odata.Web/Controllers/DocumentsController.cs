using Bialecki.com.Odata.Web.Dtos;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Mvc;

namespace Bialecki.com.Odata.Web.Controllers
{
    public class DocumentsController : Controller
    {
        // documents/sendDocument
        [HttpPost]
        public ActionResult SendDocument()
        {
            try
            {
                var requestContent = GetRequestContentAsString();
                var document = XmlHelper.XmlDeserializeFromString<DocumentDto>(requestContent);

                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch (System.Exception)
            {
                // logging
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        private string GetRequestContentAsString()
        {
            using (var receiveStream = Request.InputStream)
            {
                using (var readStream = new StreamReader(receiveStream, Encoding.UTF8))
                {
                    return readStream.ReadToEnd();
                }
            }
        }
    }
}
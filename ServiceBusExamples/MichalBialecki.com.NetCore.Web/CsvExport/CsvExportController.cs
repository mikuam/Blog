using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;

namespace MichalBialecki.com.NetCore.Web.CsvExport
{
    [Route("api/Export")]
    public class CsvExportController : Controller
    {
        private readonly ICsvExport _csvExport;

        public CsvExportController(ICsvExport csvExport)
        {
            _csvExport = csvExport;
        }

        [Route("Products")]
        [HttpGet]
        public IActionResult Products()
        {
            var data = _csvExport.ReturnData();

            var stream = new MemoryStream(Encoding.UTF8.GetBytes(data));
            var result = new FileStreamResult(stream, "text/plain");
            result.FileDownloadName = "export_" + DateTime.Now + ".csv";

            return result;
        }
    }
}
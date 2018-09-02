using MichalBialecki.com.NetCore.Web.CsvExport.Attributes;
using MichalBialecki.com.NetCore.Web.CsvExport.Data;
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
        private readonly IProductGenerator _productGenerator;

        private readonly ICsvExport _csvExport;

        private readonly IProductComparerExportService _productComparerExportService;

        private readonly IProductAnalyticsExportService _productAnalyticsExportService;

        public CsvExportController(
            IProductGenerator productGenerator,
            ICsvExport csvExport,
            IProductComparerExportService productComparerExportService,
            IProductAnalyticsExportService productAnalyticsExportService)
        {
            _productGenerator = productGenerator;
            _csvExport = csvExport;
            _productComparerExportService = productComparerExportService;
            _productAnalyticsExportService = productAnalyticsExportService;
        }

        [Route("Products")]
        [HttpGet]
        public IActionResult Products()
        {
            var products = _productGenerator.GenerateProducts(100);
            var data = _csvExport.ReturnData(products);

            var stream = new MemoryStream(Encoding.UTF8.GetBytes(data));
            var result = new FileStreamResult(stream, "text/plain");
            result.FileDownloadName = "export_" + DateTime.Now + ".csv";

            return result;
        }

        [Route("ProductComparerExport")]
        [HttpGet]
        public IActionResult ProductComparerExport()
        {
            var products = _productGenerator.GenerateProducts(100);
            var data = _productComparerExportService.Export(products);

            var stream = new MemoryStream(Encoding.UTF8.GetBytes(data));
            var result = new FileStreamResult(stream, "text/plain");
            result.FileDownloadName = "export_" + DateTime.Now + ".csv";

            return result;
        }

        [Route("ProductAnalyticsExport")]
        [HttpGet]
        public IActionResult ProductAnalyticsExport()
        {
            var products = _productGenerator.GenerateProducts(100);
            var data = _productAnalyticsExportService.Export(products);

            var stream = new MemoryStream(Encoding.UTF8.GetBytes(data));
            var result = new FileStreamResult(stream, "text/plain");
            result.FileDownloadName = "export_" + DateTime.Now + ".csv";

            return result;
        }
    }
}
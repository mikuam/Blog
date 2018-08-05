using Microsoft.AspNetCore.Mvc;

namespace MichalBialecki.com.NetCore.Web.CsvExport
{
    public class CsvExportController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
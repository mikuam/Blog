using MichalBialecki.com.NetCore.Web.CsvExport.Data;
using System.Collections.Generic;

namespace MichalBialecki.com.NetCore.Web.CsvExport
{
    public interface IProductAnalyticsExportService
    {
        string Export(IList<ProductDto> products);
    }
}

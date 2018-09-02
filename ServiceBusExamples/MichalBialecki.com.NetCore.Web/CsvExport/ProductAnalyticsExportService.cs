using MichalBialecki.com.NetCore.Web.CsvExport.Attributes;
using MichalBialecki.com.NetCore.Web.CsvExport.Data;
using System.Collections.Generic;

namespace MichalBialecki.com.NetCore.Web.CsvExport
{
    public class ProductAnalyticsExportService : GenericExportService, IProductAnalyticsExportService
    {
        public string Export(IList<ProductDto> products)
        {
            var result = Export<ProductAnalyticsAttribute>(products);

            return result;
        }
    }
}

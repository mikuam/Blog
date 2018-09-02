using MichalBialecki.com.NetCore.Web.CsvExport.Attributes;
using MichalBialecki.com.NetCore.Web.CsvExport.Data;
using System.Collections.Generic;

namespace MichalBialecki.com.NetCore.Web.CsvExport
{
    public class ProductComparerExportService : GenericExportService, IProductComparerExportService
    {
        public string Export(IList<ProductDto> products)
        {
            var result = Export<ProductComparerExportAttribute>(products);

            return result;
        }
    }
}

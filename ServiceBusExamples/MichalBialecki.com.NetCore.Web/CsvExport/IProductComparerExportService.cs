using System.Collections.Generic;
using MichalBialecki.com.NetCore.Web.CsvExport.Data;

namespace MichalBialecki.com.NetCore.Web.CsvExport
{
    public interface IProductComparerExportService
    {
        string Export(IList<ProductDto> products);
    }
}
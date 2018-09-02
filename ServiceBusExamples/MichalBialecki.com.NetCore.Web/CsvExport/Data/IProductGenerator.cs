using System.Collections.Generic;

namespace MichalBialecki.com.NetCore.Web.CsvExport.Data
{
    public interface IProductGenerator
    {
        List<ProductDto> GenerateProducts(int count);
    }
}
using MichalBialecki.com.NetCore.Web.CsvExport.Attributes;
using System.Reflection;

namespace MichalBialecki.com.NetCore.Web.CsvExport.Data
{
    public class ExportProperty
    {
        public PropertyInfo PropertyInfo { get; set; }

        public ExportAttribute ExportAttribute { get; set; }
    }
}

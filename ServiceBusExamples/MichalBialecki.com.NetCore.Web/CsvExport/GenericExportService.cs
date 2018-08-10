using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MichalBialecki.com.NetCore.Web.CsvExport.Attributes;
using MichalBialecki.com.NetCore.Web.CsvExport.Data;

namespace MichalBialecki.com.NetCore.Web.CsvExport
{
    public class GenericExportService
    {
        private const string CsvDelimeter = ";";

        public string Export<TAttribute>(IEnumerable<ProductDto> products)
            where TAttribute : ExportAttribute
        {
            using (var exportStream = (MemoryStream)GetStream<TAttribute>(products))
            {
                var encoding = new UTF8Encoding(false);
                return encoding.GetString(exportStream.ToArray());
            }
        }

        private Stream GetStream<TAttribute>(IEnumerable<ProductDto> objectList)
            where TAttribute : ExportAttribute
        {
            var stream = new MemoryStream();
            var streamWriter = new StreamWriter(stream, new UTF8Encoding(false));

            var exportProperties = GetExportProperties<TAttribute>();
            if (exportProperties != null && exportProperties.Any())
            {
                streamWriter.WriteLine(string.Join(CsvDelimeter, exportProperties.Select(p => p.ExportName)));

                objectList?.ToList().ForEach(
                    objectToExport =>
                    {
                        streamWriter.WriteLine(string.Join(CsvDelimeter, exportProperties.Select(p => p.GetValue(objectToExport))));
                    });

                streamWriter.Flush();
            }

            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }

        private List<ExportProperty> GetExportProperties<TAttribute>()
            where TAttribute : ExportAttribute
        {
            var exportProperties = typeof(ProductDto).GetProperties().Select(
                propertyInfo =>
                {
                    var exportAttribute = (TAttribute)propertyInfo.GetCustomAttributes(typeof(TAttribute), false).FirstOrDefault();

                    return exportAttribute != null
                        ? new ExportProperty
                        {
                            PropertyInfo = propertyInfo,
                            ExportName = exportAttribute.ExportName ?? propertyInfo.Name,
                            IsXmlAttribute = (exportAttribute as XmlExportAttribute)?.IsXmlAttribute ?? false,
                            ValueAsXmlCdata = (exportAttribute as XmlExportAttribute)?.ValueAsCdata ?? false,
                            ExportValue = exportAttribute.ExportValue,
                            Format = exportAttribute.Format,
                            Order = exportAttribute.Order,
                            Culture = string.IsNullOrWhiteSpace(exportAttribute.CultureName)
                                ? CultureInfo.InvariantCulture
                                : CultureInfo.CreateSpecificCulture(exportAttribute.CultureName),
                            CustomFormatProviderType = exportAttribute.CustomFormatProviderType,
                            ValueConverterType = exportAttribute.ValueConverterType
                        }
                        : null;
                }).Where(exportProperty => exportProperty != null).OrderBy(exportProperty => exportProperty.Order).ToList();

            return exportProperties;
        }
    }
}

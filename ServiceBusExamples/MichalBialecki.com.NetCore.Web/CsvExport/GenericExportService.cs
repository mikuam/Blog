using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
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
            
            var columns = GetColumnNames<TAttribute>();
            streamWriter.WriteLine(string.Join(CsvDelimeter, columns));

            foreach (var item in objectList)
            {
                var values = GetProductValues<TAttribute>(item);
                streamWriter.WriteLine(string.Join(CsvDelimeter, values));
            }

            streamWriter.Flush();
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }

        private IEnumerable<string> GetColumnNames<TAttribute>()
            where TAttribute : ExportAttribute
        {
            return typeof(ProductDto).GetProperties().Select(
                property => {
                    var exportAttribute = ((TAttribute)property.GetCustomAttributes(typeof(TAttribute), false).FirstOrDefault());
                    return exportAttribute?.ExportName;
                }).Where(p => p != null);
        }

        private List<string> GetProductValues<TAttribute>(ProductDto product)
            where TAttribute : ExportAttribute
        {
            var properties = typeof(ProductDto).GetProperties();
            var propertyValues = new List<string>();
            foreach (var propertyInfo in properties)
            {
                var attribute = (TAttribute)propertyInfo.GetCustomAttributes(typeof(TAttribute), false).FirstOrDefault();
                if (attribute != null)
                {
                    propertyValues.Add(GetAttributeValue(product, propertyInfo, attribute));
                }
            }

            return propertyValues;
        }

        private string GetAttributeValue<TAttribute>(ProductDto product, PropertyInfo propertyInfo, TAttribute attribute)
            where TAttribute : ExportAttribute
        {
            object value = propertyInfo.GetValue(product);

            if (value == null || attribute == null)
            {
                return string.Empty;
            }

            if (!string.IsNullOrWhiteSpace(attribute.Format) && value is IFormattable)
            {
                return (value as IFormattable).ToString(attribute.Format, CultureInfo.CurrentCulture);
            }

            if (!string.IsNullOrWhiteSpace(attribute.Format))
            {
                return string.Format(attribute.Format, value);
            }

            return propertyInfo.GetValue(product).ToString();
        }
    }
}

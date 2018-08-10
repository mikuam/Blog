using System;
using System.Globalization;
using System.Reflection;

namespace MichalBialecki.com.NetCore.Web.CsvExport.Data
{
    public class ExportProperty
    {
        public PropertyInfo PropertyInfo { get; set; }

        public string ExportName { get; set; }

        public string ExportValue { get; set; }

        public bool IsXmlAttribute { get; set; }

        public bool ValueAsXmlCdata { get; set; }

        public string Format { get; set; }

        public int Order { get; set; }

        public CultureInfo Culture { get; set; }

        public Type CustomFormatProviderType { get; set; }

        public Type ValueConverterType { get; set; }

        public string GetValue(object obj)
        {
            object value = PropertyInfo.GetValue(obj);

            if (!string.IsNullOrWhiteSpace(ExportValue))
            {
                return ExportValue;
            }

            if (value == null)
            {
                return string.Empty;
            }

            if (!string.IsNullOrWhiteSpace(Format) && value is IFormattable)
            {
                return (value as IFormattable).ToString(Format, Culture);
            }

            if (!string.IsNullOrWhiteSpace(Format))
            {
                return string.Format(Format, value);
            }

            return PropertyInfo.GetValue(obj).ToString();
        }
    }
}

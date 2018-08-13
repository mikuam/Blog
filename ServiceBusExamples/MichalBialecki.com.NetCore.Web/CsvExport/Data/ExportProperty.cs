using System;
using System.Globalization;
using System.Reflection;

namespace MichalBialecki.com.NetCore.Web.CsvExport.Data
{
    public class ExportProperty
    {
        public PropertyInfo PropertyInfo { get; set; }

        public string ExportName { get; set; }

        public string Format { get; set; }

        public int Order { get; set; }

        public string GetValue(object obj)
        {
            object value = PropertyInfo.GetValue(obj);

            if (value == null)
            {
                return string.Empty;
            }

            if (!string.IsNullOrWhiteSpace(Format) && value is IFormattable)
            {
                return (value as IFormattable).ToString(Format, CultureInfo.CurrentCulture);
            }

            if (!string.IsNullOrWhiteSpace(Format))
            {
                return string.Format(Format, value);
            }

            return PropertyInfo.GetValue(obj).ToString();
        }
    }
}

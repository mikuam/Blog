namespace MichalBialecki.com.NetCore.Web.CsvExport.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class, AllowMultiple = true)]
    public abstract class ExportAttribute : Attribute
    {
        public string ExportName { get; set; }

        public string Format { get; set; }

        public int Order { get; set; }
    }
}

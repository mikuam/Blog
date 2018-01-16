using System;

namespace ServiceBusExamples.MessagesSender.NetCore.Web.Dto
{
    public class DocumentDto
    {
        public string StockId { get; set; }

        public string Name { get; set; }

        public float Price { get; set; }
        
        public DateTime UpdatedAt { get; set; }
    }
}

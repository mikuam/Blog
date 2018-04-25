using System.Xml.Serialization;

namespace MichalBialecki.com.NetCore.Web.Documents
{
    [XmlRoot(ElementName = "document", Namespace = "")]
    public class DocumentDto
    {
        [XmlElement(DataType = "string", ElementName = "id")]
        public string Id { get; set; }

        [XmlElement(DataType = "string", ElementName = "content")]
        public string Content { get; set; }

        [XmlElement(DataType = "string", ElementName = "author")]
        public string Author { get; set; }

        [XmlElement(ElementName = "links")]
        public LinkDto Links { get; set; }
    }

    public class LinkDto
    {
        [XmlElement(ElementName = "link")]
        public string[] Link { get; set; }
    }
}

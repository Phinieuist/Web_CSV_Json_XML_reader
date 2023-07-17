using Newtonsoft.Json.Linq;
using System.Xml;

namespace Web_CSV_Json_XML_reader.Models
{
    public class XMLViewModel
    {
        public string Name { get; set; }
        public string RawHTML { get; set; }
        public XmlDocument Data { get; set; }

        public XMLViewModel() { }

        public XMLViewModel(XmlDocument data)
        {
            Data = data;
        }

        public XMLViewModel(XmlDocument data, string RawHTML)
        {
            Data = data;
            this.RawHTML = RawHTML;
        }

        public XMLViewModel(XmlDocument data, string RawHTML, string Name)
        {
            Data = data;
            this.RawHTML = RawHTML;
            this.Name = Name;
        }
    }
}

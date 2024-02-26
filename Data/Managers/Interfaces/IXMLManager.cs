using Newtonsoft.Json.Linq;
using System.Xml;
using Web_CSV_Json_XML_reader.ViewModels;

namespace Web_CSV_Json_XML_reader.Data.Managers.Interfaces
{
    public interface IXMLManager
    {
        public XmlDocument GetXmlDocument(HttpRequest request);

        public XMLViewModel GetXmlVM(string xmlText, string fileName);
    }
}

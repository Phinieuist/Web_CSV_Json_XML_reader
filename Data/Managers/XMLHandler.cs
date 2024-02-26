using System.Xml;
using Web_CSV_Json_XML_reader.Data.Managers.Interfaces;
using Web_CSV_Json_XML_reader.ViewModels;

namespace Web_CSV_Json_XML_reader.Data.Managers
{
    public class XMLHandler : IXMLManager
    {
        public XmlDocument GetXmlDocument(HttpRequest request)
        {
            throw new NotImplementedException();
        }

        public XMLViewModel GetXmlVM(string xmlText, string fileName)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xmlText);

            return new XMLViewModel(xmlDocument, fileName);
        }
    }
}

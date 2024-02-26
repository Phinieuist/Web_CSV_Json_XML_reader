using System.Xml;
using Web_CSV_Json_XML_reader.Data.Managers.Interfaces;
using Web_CSV_Json_XML_reader.ViewModels;

namespace Web_CSV_Json_XML_reader.Data.Managers
{
    public class XMLHandlerOld : IXMLManager
    {
        public XmlDocument GetXmlDocument(HttpRequest request)
        {
            XmlDocument xmlDocument = new();
            xmlDocument.LoadXml(request.Form["Data"]);
            XmlNode rootNode = xmlDocument.DocumentElement;

            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(xmlDocument.NameTable);
            foreach (XmlAttribute attribute in rootNode.Attributes)
            {
                if (attribute.Name.StartsWith("xmlns:"))
                {
                    string prefix = attribute.LocalName;
                    string ns = attribute.Value;
                    namespaceManager.AddNamespace(prefix, ns);
                }
            }

            List<string> Keys = request.Form.Keys.ToList();
            Keys.Remove("Name");
            Keys.Remove("Data");

            foreach (string key in Keys)
            {
                XmlNode node = xmlDocument.SelectSingleNode(key, namespaceManager);

                if (node != null)
                    switch (node.NodeType)
                    {
                        case XmlNodeType.Attribute:
                            node.Value = Convert.ToString(request.Form[key]);
                            break;
                        case XmlNodeType.Element:
                            node.InnerText = Convert.ToString(request.Form[key]);
                            break;
                        default:
                            continue;
                    }
            }

            return xmlDocument;
        }

        public XMLViewModel GetXmlVM(string xmlText, string fileName)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xmlText);

            return new XMLViewModel(xmlDocument, fileName);
        }
    }
}

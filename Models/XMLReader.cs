using Microsoft.Extensions.Primitives;
using System;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Web_CSV_Json_XML_reader.Models
{
    public class XMLReader
    {
        public static string Read(XmlDocument xmlDocument)
        {
            XmlNode rootNode = xmlDocument.DocumentElement;

            return ProcessNode(rootNode, 0);
        }

        static string ProcessNode(XmlNode node, int level)
        {
            StringBuilder sb = new StringBuilder();

            if (node.NodeType == XmlNodeType.Element)
            {
                string type = "text";
                sb.Append("<details>");
                sb.Append($"<summary>&lt;{node.Name}&gt;</summary>");

                if (node.Attributes != null)
                {
                    foreach (XmlAttribute attribute in node.Attributes)
                    {
                        sb.Append($"<li>{attribute.Name} : <input type='{type}' name='{FindXPath(attribute)}' value='{attribute.Value}'/></li>");
                    }

                    sb.Append("</ul>");
                }

                if (node.HasChildNodes)
                {
                    for (int i = 0; i < node.ChildNodes.Count; i++)
                    {
                        sb.Append(ProcessNode(node.ChildNodes[i], level + 1));
                    }
                }

                sb.Append("</details>");
            }
            else if (node.NodeType == XmlNodeType.Text)
            {
                sb.Append($"<p>{new string(' ', level * 4)} <input type='text' name='{FindXPath(node)}' value='{node.InnerText}' /></p>");
            }

            return sb.ToString();
        }

        private static string FindXPath(XmlNode node)
        {
            StringBuilder builder = new StringBuilder();
            while (node != null)
            {
                switch (node.NodeType)
                {
                    case XmlNodeType.Attribute:
                        builder.Insert(0, "/@" + node.Name);
                        node = ((XmlAttribute)node).OwnerElement;
                        break;
                    case XmlNodeType.Element:
                        int index = FindElementIndex((XmlElement)node);
                        builder.Insert(0, "/" + node.Name + "[" + index + "]");
                        node = node.ParentNode;
                        break;
                    case XmlNodeType.Text:
                        builder.Insert(0, "");
                        node = node.ParentNode;
                        break;
                    case XmlNodeType.Document:
                        return builder.ToString();                  
                    default:
                        throw new ArgumentException("Only elements and attributes are supported");
                }
            }
            throw new ArgumentException("Node was not in a document");
        }

        private static int FindElementIndex(XmlElement element)
        {
            XmlNode parentNode = element.ParentNode;
            if (parentNode is XmlDocument)
            {
                return 1;
            }
            XmlElement parent = (XmlElement)parentNode;
            int index = 1;
            foreach (XmlNode candidate in parent.ChildNodes)
            {
                if (candidate is XmlElement && candidate.Name == element.Name)
                {
                    if (candidate == element)
                    {
                        return index;
                    }
                    index++;
                }
            }
            throw new ArgumentException("Couldn't find element within parent");
        }

        public XMLReader() { }
    }
}

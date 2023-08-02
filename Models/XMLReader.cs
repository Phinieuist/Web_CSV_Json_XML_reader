using Microsoft.Extensions.Primitives;
using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Web_CSV_Json_XML_reader.Models
{
    public class XMLReader
    {
        public static string Read(string xmlDocument, out XmlDocument xmlD)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xmlDocument);
                xmlD = xmlDoc;

                XmlNode rootNode = xmlDoc.DocumentElement;

                return ProcessNode(rootNode, 0);
            }
            catch(Exception ex)
            {
                throw new Exception("Ошибка в процессе чтения XML-документа. " + ex.Message);
            }
        }

        private static string ProcessNode(XmlNode node, int level)
        {
            StringBuilder sb = new StringBuilder();

            if (node.NodeType == XmlNodeType.Element)
            {
                sb.Append($"<p><button class='btn btn-primary' type='button' data-bs-toggle='collapse' data-bs-target='#ID_{FindXPath(node)}' aria-expanded='false' aria-controls='ID_{FindXPath(node)}'>Object [{node.Name}]</button></p>");
                sb.Append($"<div class='collapse' id='ID_{FindXPath(node)}'>");

                if (node.Attributes != null)
                {
                    sb.Append("<ul>");
                    foreach (XmlAttribute attribute in node.Attributes)
                    {
                        sb.Append($"<li><div class='input-group mb-3'><div class='input-group-prepend'><span class='input-group-text'>{attribute.Name}</span></div> <textarea name='{FindXPath(attribute)}' class='form-control textInp'>{attribute.Value}</textarea></div></li>");
                    }

                    sb.Append("</ul>");
                }

                if (node.HasChildNodes)
                {
                    sb.Append("<ul>");
                    for (int i = 0; i < node.ChildNodes.Count; i++)
                    {
                        sb.Append("<li>");
                        sb.Append(ProcessNode(node.ChildNodes[i], level + 1));
                        sb.Append("</li>");
                    }
                    sb.Append("</ul>");
                }
                sb.Append("</div>");
            }
            else if (node.NodeType == XmlNodeType.Text)
            {
                sb.Append($"<p><textarea name='{FindXPath(node)}' class='form-control mb-3 textInp'>{node.InnerText}</textarea></p>");
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
                        throw new ArgumentException("Поддерживаются только элементы и атрибуты");
                }
            }
            throw new ArgumentException("Узел не найден в документе");
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
            throw new ArgumentException("Индекс не найден");
        }

        public XMLReader() { }
    }
}

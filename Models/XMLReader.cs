using System.Xml;

namespace Web_CSV_Json_XML_reader.Models
{
    public class XMLReader
    {
        public static string Read(string XMLtext)
        {
            string Output = "";

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(XMLtext);

            XmlNode rootNode = xmlDocument.DocumentElement;
            Output = ProcessNode(rootNode, 0);

            return Output;
        }

        static string ProcessNode(XmlNode node, int level, string str)
        {
            if (node.NodeType == XmlNodeType.Element)
            {
                str += new string(' ', level * 2) + "<" + node.Name + ">\n";

                if (node.Attributes != null)
                {
                    foreach (XmlAttribute attribute in node.Attributes)
                    {
                        str += new string(' ', (level + 1) * 2) + "@" + attribute.Name + ": " + attribute.Value + "\n";
                    }
                }

                if (node.HasChildNodes)
                {
                    foreach (XmlNode childNode in node.ChildNodes)
                    {
                        str += ProcessNode(childNode, level + 1, str);
                    }
                }

                str += new string(' ', level * 2) + "</" + node.Name + ">\n";
            }
            else if (node.NodeType == XmlNodeType.Text)
            {
                str+= new string(' ', level * 2) + node.InnerText + "\n";
            }

            return str;
        }

        static string ProcessNode(XmlNode node, int level)
        {
            string result = "";

            if (node.NodeType == XmlNodeType.Element)
            {
                result += "<details>";
                result += $"<summary>&lt;{node.Name}&gt;</summary>";

                if (node.Attributes != null)
                {
                    result += "<ul>";
                    foreach (XmlAttribute attribute in node.Attributes)
                    {
                        result += $"<li>{attribute.Name}: {attribute.Value}</li>";
                    }
                    result += "</ul>";
                }

                if (node.HasChildNodes)
                {
                    foreach (XmlNode childNode in node.ChildNodes)
                    {
                        result += ProcessNode(childNode, level + 1);
                    }
                }

                result += "</details>";
            }
            else if (node.NodeType == XmlNodeType.Text)
            {
                result += $"<p>{new string(' ', level * 4)}{node.InnerText}</p>";
            }

            return result;
        }

        public XMLReader() { }
    }
}

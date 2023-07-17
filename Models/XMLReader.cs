using Microsoft.Extensions.Primitives;
using System;
using System.Reflection.Emit;
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

            return ProcessNode(rootNode, 0, "root");
        }

        static object Deserialize(XmlNode node)
        {
            // Проверка типа узла
            switch (node.NodeType)
            {
                case XmlNodeType.Element:
                    // Если узел является элементом, создаём новый словарь
                    var dict = new Dictionary<string, object>();

                    // Перебираем все атрибуты элемента и добавляем их в словарь
                    foreach (XmlAttribute attribute in node.Attributes)
                    {
                        dict[attribute.Name] = attribute.Value;
                    }

                    // Перебираем все дочерние узлы элемента
                    foreach (XmlNode childNode in node.ChildNodes)
                    {
                        // Выполняем рекурсивную десериализацию каждого дочернего узла
                        var childObject = Deserialize(childNode);

                        // Если уже существует значение с таким же ключом,
                        // то преобразуем его в список значений
                        if (dict.ContainsKey(childNode.Name))
                        {
                            if (dict[childNode.Name] is List<object> list)
                            {
                                list.Add(childObject);
                            }
                            else
                            {
                                dict[childNode.Name] = new List<object> { dict[childNode.Name], childObject };
                            }
                        }
                        else
                        {
                            dict[childNode.Name] = childObject;
                        }
                    }

                    return dict;

                case XmlNodeType.Text:
                case XmlNodeType.CDATA:
                    // Если узел содержит текст или CDATA, возвращаем его значение
                    return node.Value;

                default:
                    // Возвращаем null для всех остальных типов узлов
                    return null;
            }
        }

        static string ProcessNode3(XmlNode node, int level)
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
                        result += "<li>";
                        result += $"<label>{attribute.Name}: </label>";
                        result += $"<input type='text' id='{attribute.Name}' value='{attribute.Value}' onchange='updateAttributeValue(\"{attribute.Name}\", this.value)' />";
                        result += "</li>";
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

        static string ProcessNode2(XmlNode node, int level)
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
                        result += "<li>";
                        result += $"<label>{attribute.Name}: </label>";
                        result += $"<input type='text' value='{attribute.Value}' onchange='updateAttribute(this, \"{attribute.Name}\")' />";
                        result += "</li>";
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

        static string ProcessNode(XmlNode node, int level, string prefix = "")
        {
            StringBuilder sb = new StringBuilder();

            if (node.NodeType == XmlNodeType.Element)
            {
                string type = "text";
                sb.Append("<details>");
                sb.Append($"<summary>&lt;{node.Name}&gt;</summary>");
                //$"<input type='{type}' name='{prefix}' value='{val.Value}'/>"
                
                if (node.Attributes != null)
                {
                    foreach (XmlAttribute attribute in node.Attributes)
                    {
                        sb.Append($"<li>{attribute.Name} : <input type='{type}' name='{prefix}.{attribute.Name}' value='{attribute.Value}'/></li>");
                    }


                    //for (int i = 0;i< node.Attributes.Count; i++)
                    //{
                    //    sb.Append($"<li>{node.Attributes[i].Name} : <input type='{type}' name='{prefix}.{node.Attributes[i].Name}[{i}]' value='{node.Attributes[i].Value}'/></li>");
                    //}
                    sb.Append("</ul>");
                }

                if (node.HasChildNodes)
                {
                    //foreach (XmlNode childNode in node.ChildNodes)
                    //{
                    //    sb.Append(ProcessNode(childNode, level + 1, $"{prefix}.{node.Name}"));
                    //}

                    //for (int i = 0; i < node.ChildNodes.Count; i++)
                    //{                     
                    //    sb.Append(ProcessNode(node.ChildNodes[i], level + 1, $"{prefix}.{node.ChildNodes[i].Name}[{i}]"));
                    //}

                    for (int i = 0; i < node.ChildNodes.Count; i++)
                    {
                        sb.Append(ProcessNode(node.ChildNodes[i], level + 1, $"{prefix}.level[{level}][{i}]"));
                    }
                }

                sb.Append("</details>");
            }
            else if (node.NodeType == XmlNodeType.Text)
            {
                sb.Append($"<p>{new string(' ', level * 4)}{node.InnerText}</p>");
            }

            return sb.ToString();
        }

        public XMLReader() { }
    }
}
